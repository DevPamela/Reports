using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Reports.Application.Middlewares
{
    public class SessionExpirationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SessionExpirationMiddleware> _logger;
        private readonly int _sessionTimeoutMinutes;

        public SessionExpirationMiddleware(RequestDelegate next, ILogger<SessionExpirationMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _sessionTimeoutMinutes = configuration.GetValue<int>("Session:IdleTimeoutMinutes");
        }

        public async Task Invoke(HttpContext context)
        {
            var user = context.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                var sessionLastActive = context.Session.GetString("LastActiveTime");

                if (!string.IsNullOrEmpty(sessionLastActive) && DateTime.TryParse(sessionLastActive, out var lastActiveTime))
                {
                    var idleTimeLimit = TimeSpan.FromMinutes(_sessionTimeoutMinutes);

                    if (DateTime.UtcNow - lastActiveTime > idleTimeLimit)
                    {
                        _logger.LogInformation("Session expired for user {User}. Logging out.", user.Identity.Name);

                        context.Session.Clear();
                        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }

                // Update session last active time
                context.Session.SetString("LastActiveTime", DateTime.UtcNow.ToString());
            }

            await _next(context);
        }
    }
}
