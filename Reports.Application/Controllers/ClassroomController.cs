using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Interfaces.Service;

namespace Reports.Application.Controllers
{
    [Authorize]
    public class ClassroomController : BaseController
    {
        private readonly IClassroomService _classroomService;
        private readonly ILogger<ClassroomController> _logger;

        public ClassroomController(IClassroomService classroomService, ILogger<ClassroomController> logger) : base(logger)
        {
            _classroomService = classroomService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> SelectClassroomForReport()
        {
            var classroom = await _classroomService.GetAllAsync();

            var classModel = new ClassroomViewModel
            {
                Classes = classroom
            };

            return View(classModel);
        }

        [HttpPost]
        public async Task<ActionResult> SetIsSelectedPropertyAstrue(List<int> SelectedClasses)
        {
            foreach (var classroom in SelectedClasses)
            {
                var classes = await _classroomService.GetById(classroom);
                await _classroomService.SetClassroomAsTrue(classes);
            }
            TempData["SelectedClasses"] = SelectedClasses;

            return RedirectToAction("SelectStudentsForReport", "Student");
        }
    }
}
