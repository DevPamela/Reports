using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Interfaces.Service;
using System.Runtime.CompilerServices;

namespace Reports.Application.Controllers
{
    public class ClassroomController : Controller
    {
        private readonly IClassroomService _classroomService;

        public ClassroomController(IClassroomService classroomService)
        {
            _classroomService = classroomService;
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
        public async Task<ActionResult> SetIsSelectedPropertyAstrue(List<int>SelectedClasses)
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
