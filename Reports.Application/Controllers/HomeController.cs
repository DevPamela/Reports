using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Interfaces.Service;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Reports.Application.Enums;

namespace Reports.Application.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IClassroomService _classroomService;
        ITeacherService _teacherService;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IClassroomService classroomService, ITeacherService teacherService, ILogger<HomeController> logger) : base(logger)
        {
            _classroomService = classroomService;
            _teacherService = teacherService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Processing request in Home Index - Welcome to the application!");
            var home = new HomeModel();
            if (User != null && User.Identity.IsAuthenticated)
            {
                var userid = int.Parse(User.FindFirst("UserId")?.Value);
                if (userid > 0)
                {
                    home.Teacher = await _teacherService.GetByUserIdAsync(userid);
                    home.Classrooms = await _classroomService.GetByTeacherId(userid);
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
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = statusCode,
                Message = message ?? "Ocorreu um erro inesperado."
            };
            return View(model);
        }
    }
}