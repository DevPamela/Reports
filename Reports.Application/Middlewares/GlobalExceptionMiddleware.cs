namespace Reports.Application.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Continue to the next middleware or endpoint
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred during the request.");

                // Redirect to error page with custom message and 500 status code
                await HandleExceptionAsync(context, ex);
            }

            if (context.Response.StatusCode >= 400)
            {
                // Handle specific status codes (e.g., 404, 403)
                await HandleStatusCodeAsync(context);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.Redirect($"/Home/Error?statusCode=500&message={Uri.EscapeDataString(ex.Message)}");
            return Task.CompletedTask;
        }

        private Task HandleStatusCodeAsync(HttpContext context)
        {
            var statusCode = context.Response.StatusCode;
            var message = statusCode switch
            {
                400 => "A solicitação enviada contém erros ou está inválida.",
                401 => "Você precisa estar autenticado para acessar este recurso.",
                403 => "Você não tem permissão para acessar este recurso.",
                404 => "A página que você está procurando não foi encontrada.",
                408 => "O tempo de solicitação expirou. Por favor, tente novamente.",
                429 => "Você fez muitas solicitações em um curto período. Aguarde e tente novamente.",
                500 => "Ocorreu um erro interno no servidor. Tente novamente mais tarde.",
                501 => "O servidor não suporta a funcionalidade necessária para concluir sua solicitação.",
                502 => "O servidor recebeu uma resposta inválida do servidor upstream.",
                503 => "O servidor está temporariamente indisponível. Tente novamente mais tarde.",
                504 => "O servidor demorou muito para responder. Por favor, tente novamente.",
                _ => "Ocorreu um erro inesperado."
            };
            context.Response.Redirect($"/Home/Error?statusCode={statusCode}&message={Uri.EscapeDataString(message)}");
            return Task.CompletedTask;
        }
    }
}
