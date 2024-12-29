namespace Reports.Application.Middlewares
{
    public class NotificationMiddleware
    {
        private readonly RequestDelegate _next;

        public NotificationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Items.TryGetValue("Notification", out var notification))
            {
                // Add notifications to the response headers
                context.Response.Headers.Add("X-Notification", notification.ToString());
            }

            await _next(context);
        }
    }
}
