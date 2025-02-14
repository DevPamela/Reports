using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Entities;
using Reports.Domain.Interfaces.Service;

namespace Reports.Application.Controllers
{
    [Authorize]
    public class TeacherController : BaseController<TeacherController>
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService, ILogger<TeacherController> logger) : base(logger)
        {
            _teacherService = teacherService;
        }

        public IActionResult Index()
        {
            var userId = GetUserIdFromSession();
            Logger.LogInformation("User {UserId} navigated to Teacher Index page.", userId);
            return View();
        }

        public IActionResult AddTeacher()
        {
            var userId = GetUserIdFromSession();
            Logger.LogInformation("User {UserId} navigated to Add Teacher page.", userId);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RegisterNewTeacher(TeacherViewModel teacherViewModel)
        {
            var userId = GetUserIdFromSession();

            if (!ModelState.IsValid)
            {
                Logger.LogWarning("User {UserId} submitted invalid data for RegisterNewTeacher: {@ModelStateErrors}", userId, ModelState.Values.SelectMany(v => v.Errors));
                return View("AddTeacher", teacherViewModel);
            }

            try
            {
                Logger.LogInformation("User {UserId} is attempting to register a new teacher: {@TeacherViewModel}", userId, teacherViewModel);

                var teacher = new Teacher
                {
                    Name = teacherViewModel.Name,
                    UserId = int.Parse(userId)
                };

                await _teacherService.RegisterTeacher(teacher);

                Logger.LogInformation("User {UserId} successfully registered a new teacher with Name: {Name}", userId, teacher.Name);
                return RedirectToAction("Index", "Student");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred for User {UserId} while registering a new teacher: {@TeacherViewModel}", userId, teacherViewModel);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View("AddTeacher", teacherViewModel);
            }
        }
    }
}