using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Interfaces.Service;

namespace Reports.Application.Controllers
{
    [Authorize]
    public class ClassroomController : BaseController<ClassroomController>
    {
        private readonly IClassroomService _classroomService;

        public ClassroomController(IClassroomService classroomService, ILogger<ClassroomController> logger) : base(logger)
        {
            _classroomService = classroomService;
        }

        public IActionResult Index()
        {
            Logger.LogInformation("Accessing Classroom Index page.");
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> SelectClassroomForReport()
        {
            Logger.LogInformation("Fetching all classrooms for report selection.");
            try
            {
                var userId = GetUserIdFromSession();
                var classrooms = await _classroomService.GetByTeacherId(int.Parse(userId));
                var ClassroomViewModel = classrooms.Select(s => new ClassroomViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                }).ToList();

                
                Logger.LogInformation("Successfully fetched {ClassroomCount} classrooms.", classrooms.Count);
                var classModel = new ClassroomViewModelList
                {
                    Classes = ClassroomViewModel
                };

                return View(classModel);
            }
            catch (Exception ex)
            {
                var userId = GetUserIdFromSession();
                Logger.LogError(ex, "An error occurred for User {UserId} while fetching classrooms for report selection.", userId);
                return await ErrorAsync(null, "Failed to fetch classrooms for the report.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> SetIsSelectedPropertyAstrue(List<int> selectedClasses)
        {
            Logger.LogInformation("Setting 'IsSelected' property for {ClassCount} classrooms.", selectedClasses.Count);
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("SelectStudentsForReport", "Student");
                }

                if (selectedClasses == null || selectedClasses.Count == 0)
                {
                    ModelState.AddModelError("SelectedClasses", "Selecione pelo menos uma turma.");
                    return RedirectToAction("SelectStudentsForReport", "Classroom");
                }

                foreach (var classroomId in selectedClasses)
                {
                    var classroom = await _classroomService.GetById(classroomId);
                    await _classroomService.SetClassroomAsTrue(classroom);
                }

                Logger.LogInformation("Successfully updated 'IsSelected' property for classrooms.");
                TempData["SelectedClasses"] = selectedClasses;

                return RedirectToAction("SelectStudentsForReport", "Student");
            }
            catch (Exception ex)
            {
                var userId = GetUserIdFromSession();
                Logger.LogError(ex, "An error occurred for User {UserId} while updating 'IsSelected' property for classrooms.", userId);
                return await ErrorAsync(null, "Failed to update 'IsSelected' property for classrooms.");
            }
        }
    }
}