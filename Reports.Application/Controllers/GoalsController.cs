using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Entities;
using Reports.Domain.Interfaces.Service;

namespace Reports.Application.Controllers
{
    [Authorize]
    public class GoalsController : BaseController<GoalsController>
    {
        private readonly IGoalService _goalService;
        private readonly IAssessmentService _assessmentService;

        public GoalsController(IGoalService goalService, IAssessmentService assessmentService, ILogger<GoalsController> logger) : base(logger)
        {
            _goalService = goalService;
            _assessmentService = assessmentService;
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
            ViewBag.ValorDoBotao = "Enviar";
            return View(viewModel);
            //TODO criar botão de voltar
        }

        public async Task<ActionResult> SelectGoalsForReport()
        {
            Logger.LogInformation("Fetching goals list for report selection.");
            try
            {
                var goals = await _goalService.GetGoalsListAsync();
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
                var students = await _goalService.SaveStudentsGoalsAsync(goals);
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

                for (int i = 0; i < goalsViewModels.Count; i++)
                {
                    var collectionName = goalsViewModels[i].collectionName;
                    var description = goalsViewModels[i].Description;

                    await _goalService.AddGoalInColletionAsync(collectionName, goalName, description);
                }

                Logger.LogInformation("Successfully added goals with name {GoalName}.", goalName);
                TempData["SuccessMessage"] = "Cadastro realizado com sucesso!";
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
                await _assessmentService.AddIsAtendedInformation(students);
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
    }
}
