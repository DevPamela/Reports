using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Reports.Application.Controllers
{
    [Authorize]
    public class TeacherController : BaseController
    {
        private readonly ITeacherService _teacherService;
        private readonly ILogger<TeacherController> _logger;

        public TeacherController(ITeacherService teacherService, ILogger<TeacherController> logger): base(logger)
        {
            _teacherService = teacherService;
            _logger = logger;
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
        public async Task<ActionResult> RegisterNewTeacher(TeacherViewModel teacherViewModel)
        {
            var userId = User.FindFirst("UserId")?.Value;// se primeiro acesso tutorial básico(guia)
            var teacher = new Teacher//não forçar a entrar direto em cadastrar turma
            {
                Name = teacherViewModel.Name,
                UserId = int.Parse(userId)
            };

            await _teacherService.RegisterTeacher(teacher);

            return RedirectToAction("Index", "Student");
        }
    }
}
