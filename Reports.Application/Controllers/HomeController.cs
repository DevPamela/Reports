using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Interfaces.Service;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace Reports.Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClassroomService _classroomService;
        ITeacherService _teacherService;
        public HomeController(IClassroomService classroomService, ITeacherService teacherService)
        {
            _classroomService = classroomService;
            _teacherService = teacherService;
        }

        public async Task<IActionResult> Index()
        {
            var home = new HomeModel();
            if (User != null && User.Identity.IsAuthenticated)
            {
                var userid = int.Parse(User.FindFirst("UserId")?.Value);
                if (userid > 0)
                {
                    home.Teacher = await _teacherService.GetByUserIdAsync(userid);
                    var teacherClass = await _classroomService.GetByTeacherId(userid);
                    if (teacherClass.Count != 0)
                        home.HasClassRoom = true;
                }
            }

            return View(home);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null, string? message = null)
        {
            var errorMessage = message;

            if (statusCode.HasValue && string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = statusCode switch
                {
                    404 => "A página que você está procurando não foi encontrada.",
                    500 => "Ocorreu um erro interno no servidor. Tente novamente mais tarde.",
                    403 => "Você não tem permissão para acessar este recurso.",
                    401 => "Você precisa estar autenticado para acessar este recurso.",
                    400 => "A solicitação enviada contém erros ou está inválida.",
                    _ => "Ocorreu um erro inesperado."
                };
            }
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = statusCode,
                Message = errorMessage
            };
            return View(model);
        }
    }
}