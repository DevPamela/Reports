using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Interfaces.Service;

namespace Reports.Application.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        private readonly IClassroomService _classroomService;
        private readonly ITeacherService _teacherService;

        public HomeController(
            IClassroomService classroomService,
            ITeacherService teacherService,
            ILogger<HomeController> logger) : base(logger)
        {
            _classroomService = classroomService;
            _teacherService = teacherService;
        }

        public async Task<IActionResult> Index()
        {
            Logger.LogInformation("Home Index page accessed.");
            var home = new HomeModel();

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userId = int.Parse(User.FindFirst("UserId")?.Value);
                    home.Teacher = await _teacherService.GetByUserIdAsync(userId);
                    home.Classrooms = await _classroomService.GetByTeacherId(userId);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while loading Home Index.");
                return Error(null, "Failed to load Home Index.");
            }

            return View(home);
        }

        public IActionResult Privacy()
        {
            Logger.LogInformation("Privacy page accessed.");
            return View();
        }
    }
}
