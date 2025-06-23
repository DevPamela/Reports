using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Entities;
using Reports.Domain.Interfaces.Service;
using Reports.Service.Classroom;
using Reports.Service.Students;

namespace Reports.Application.Controllers
{
    [Authorize]
    public class GoalsController : BaseController<GoalsController>
    {
        private readonly IGoalService _goalService;
        private readonly IAssessmentService _assessmentService;
                private readonly ITeacherService _teacherService;

        public GoalsController(IGoalService goalService, IAssessmentService assessmentService, 
            ILogger<GoalsController> logger, ITeacherService teacherService) : base(logger)
        { 
            _goalService = goalService;
            _assessmentService = assessmentService;
            _teacherService = teacherService;
        }

        public IActionResult Index()
        {
            Logger.LogInformation("Accessing Goals Index page.");
            var viewModel = new List<GoalsViewModel>
            {
                new GoalsViewModel(),
                new GoalsViewModel(),
                new GoalsViewModel()
            };
          
            return View(viewModel);
        }
                
        public async Task<ActionResult> SelectGoalsForReport()
        {
            Logger.LogInformation("Fetching goals list for report selection.");
            try
            {
                var user = GetUserIdFromSession();
            var teacher = await _teacherService.GetByUserIdAsync(int.Parse(user));
            var goals = await _goalService.GetGoalsListAsyncByTeacherId(teacher.Id);

            var goalsName = goals.Select(g => g.Name).ToList();

                Logger.LogInformation("Successfully retrieved {GoalCount} goals.", goalsName.Count);
                ViewBag.Objetivos = goalsName;
                return View(goalsName);
            }
            catch (Exception ex)
            {
                var userId = GetUserIdFromSession();
                Logger.LogError(ex, "An error occurred for User {UserId} while fetching goals for report.", userId);
                return await ErrorAsync(null, "Failed to fetch goals for the report.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveStudentGoalsResults(List<string> goals)
        {
            Logger.LogInformation("Saving student goals results for {GoalCount} goals.", goals.Count);
            try
            {
                var userId = GetUserIdFromSession();
                var students = await _goalService.SaveStudentsGoalsAsync(goals, int.Parse(userId));
                Logger.LogInformation("Successfully saved goals results for students.");
                return View(students);
            }
            catch (Exception ex)
            {
                var userId = GetUserIdFromSession();
                Logger.LogError(ex, "An error occurred for User {UserId} while saving student goals results.", userId);
                return await ErrorAsync(null, "Failed to save student goals results.");
            }
        }
       
        [HttpPost]
        public async Task<ActionResult> AddGoal(List<GoalsViewModel> goalsViewModels)
        {
            Logger.LogInformation("Adding goals to the database.");
            try
            {
                var goalName = goalsViewModels[0].Name;
              
              var user = GetUserIdFromSession();
                var teacher = await _teacherService.GetByUserIdAsync(int.Parse(user));
                foreach (var goalViewModel in goalsViewModels)
                {
                    var collectionName = goalViewModel.collectionName;
                    var description = goalViewModel.Description;
                    var isGirlsGoal = goalViewModel.IsGirl;

                    await _goalService.AddGoalsAsync(collectionName, goalName, description, isGirlsGoal, teacher.Id);
                }

                Logger.LogInformation("Successfully added goals with name {GoalName}.", goalName);
                SetNotification("Paragrafo criado com sucesso!", Enums.NotificationType.Success);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var userId = GetUserIdFromSession();
                Logger.LogError(ex, "An error occurred for User {UserId} while adding goals.", userId);
                TempData["ErrorMessage"] = $"Encontramos um problema ao cadastrar seu objetivo! Por favor tente novamente.";
                return View("Index", goalsViewModels);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveIsAtendedInformation(List<Student> students)
        {
            Logger.LogInformation("Saving 'Is Attended' information for {StudentCount} students.", students.Count);
            try
            {
                var userId = GetUserIdFromSession();
                await _assessmentService.AddIsAtendedInformation(students, int.Parse(userId));
                Logger.LogInformation("Successfully saved 'Is Attended' information.");
                return View();
            }
            catch (Exception ex)
            {
                var userId = GetUserIdFromSession();
                Logger.LogError(ex, "An error occurred for User {UserId} while saving 'Is Attended' information.", userId);
                return await ErrorAsync(null, "Failed to save 'Is Attended' information.");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GoalsList()
        {
            var userId = GetUserIdFromSession();
            Logger.LogInformation($"Retrieving the student list for teacher id: {userId}.");

            var goals = await _goalService.GetGoalsListAsyncByTeacherId(int.Parse(userId));
         
            return View(goals);
        }

        [HttpGet]
        public async Task<ActionResult> EditGoal(int id)
        {
            Logger.LogInformation("Accessing edit page for Goals with ID: {GoalId}", id);

            var userId = GetUserIdFromSession();
            var teacherId = await _teacherService.GetByUserIdAsync(int.Parse(userId));
            var goal = await _goalService.GetGoalByIdAsync(id);

            return View(goal);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateGoal(Goal goal)
        {
            Logger.LogInformation("Updating goals to the database.");
            try
            {
                var user = GetUserIdFromSession();
                var teacher = await _teacherService.GetByUserIdAsync(int.Parse(user));
               
                await _goalService.UpdateStudent(goal, teacher.Id);
             
                Logger.LogInformation($"Successfully updated goal with name {goal.Name}.");
                SetNotification("Paragrafo editado com sucesso!", Enums.NotificationType.Success);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                var userId = GetUserIdFromSession();
                Logger.LogError(ex, "An error occurred for User {UserId} while adding goals.", userId);
                TempData["ErrorMessage"] = $"Encontramos um problema ao cadastrar seu objetivo! Por favor tente novamente.";
                return View("Index");
            }
        }
    }
}