using Reports.Domain.Interfaces.Service;

public class ValidacaoUsuarioMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITeacherService _teacherService;

    public ValidacaoUsuarioMiddleware(RequestDelegate next, ITeacherService teacherService)
    {
        _next = next;
        _teacherService = teacherService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Verifica se o usuário está autenticado
        if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
        {
            var userId = int.Parse(context.User.FindFirst("UserId")?.Value);

            if (userId != null)
            {
                // Recupera o usuário do banco de dados
                var usuario = await _teacherService.GetByUserIdAsync(userId);

                if (usuario == null && usuario.Students == null)
                {
                    // Redireciona para preencher dados caso as informações não estejam completas
                    context.Items["MostrarMenuCompleto"] = false;
                  
                }
                else
                {
                    context.Items["MostrarMenuCompleto"] = true;
                }
            }
        }

        // Passa para o próximo middleware
        await _next(context);
    }
}