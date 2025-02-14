using Serilog;
using Serilog.Context;

namespace Reports.Application.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private const string DefaultCorrelationIdHeader = "X-Correlation-ID";
        private readonly RequestDelegate _next;
        private readonly string _correlationIdHeader;

        public CorrelationIdMiddleware(RequestDelegate next, string correlationIdHeader = DefaultCorrelationIdHeader)
        {
            _next = next;
            _correlationIdHeader = correlationIdHeader;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var requestPath = context.Request.Path.Value?.ToLowerInvariant();
                var fullPath = $"{requestPath}{context.Request.QueryString}";

                var excludedPaths = new[] { "/user/login", "/user/authenticate", "/static/" };

                // Check for excluded paths
                if (excludedPaths.Any(p => requestPath != null && requestPath.StartsWith(p.ToLowerInvariant())))
                {
                    await _next(context);
                    return;
                }

                // Validate or generate CorrelationId
                if (!context.Request.Headers.TryGetValue(_correlationIdHeader, out var correlationId) || string.IsNullOrWhiteSpace(correlationId) || !IsValidCorrelationId(correlationId))
                {
                    correlationId = Guid.NewGuid().ToString();
                    context.Request.Headers[_correlationIdHeader] = correlationId;
                    Log.Warning("Invalid or missing CorrelationId. Generated a new one: {CorrelationId}", correlationId);
                }

                // Set CorrelationId in response headers
                context.Response.Headers["X-Correlation-ID"] = correlationId;

                using (LogContext.PushProperty("CorrelationId", correlationId))
                {
                    Log.Information("CorrelationIdMiddleware started for {FullPath}", fullPath);
                    try
                    {
                        await _next(context);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "An error occurred during request processing for {FullPath}", fullPath);
                        throw; // Rethrow the exception to ensure proper behavior
                    }
                    Log.Information("CorrelationIdMiddleware ended for {FullPath}", fullPath);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred in CorrelationIdMiddleware.");
                throw; // Ensure exception propagates
            }
        }

        private bool IsValidCorrelationId(string correlationId)
        {
            return Guid.TryParse(correlationId, out _);
        }
    }
}
