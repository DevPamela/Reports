using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Models;

namespace Reports.Application.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddTeacher()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterNewTeacher(TeacherViewModel teacherViewModel)
        {
            var teacher = new Teacher
            {
                Name = teacherViewModel.Name
            };

            await _teacherService.RegisterTeacher(teacher);

            return View("Index");
        }
    }
}
