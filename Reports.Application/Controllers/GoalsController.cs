using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Models;

namespace Reports.Application.Controllers
{
    public class GoalsController : Controller
    {
        private readonly IGoalService _goalService;
        private readonly IAssessmentService _assessmentService;
        public GoalsController(IGoalService goalService, IAssessmentService assessmentService)
        {
            _goalService = goalService;
            _assessmentService = assessmentService;
        }

        public IActionResult Index()
        {
            var viewModel = new List<GoalsViewModel> { new GoalsViewModel(), new GoalsViewModel(), new GoalsViewModel() };
            ViewBag.ValorDoBotao = "Enviar";
            return View(viewModel);
            //TODO criar botão de voltar
        }
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SelectGoalsForReport()
        {
            var goals = await _goalService.GetGoalsListAsync();
            var goalsName = goals.Select(g => g.Name).ToList();

            ViewBag.Objetivos = goalsName;
            return View(goalsName);
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveStudentGoalsResults(List<string> goals)
        {
            var students = await _goalService.SaveStudentsGoalsAsync(goals);

            return View(students);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddGoal(List<GoalsViewModel> goalsViewModels)
        {
            try
            {
                var goalName = goalsViewModels[0].Name;

                for (int i = 0; i < goalsViewModels.Count; i++)
                {
                    var collectionName = goalsViewModels[i].collectionName;
                    var description = goalsViewModels[i].Description;

                    await _goalService.AddGoalInColletionAsync(collectionName, goalName.ToString(), description);
                }

                TempData[("SuccessMessage")] = "Cadastro realizado com sucesso!";
                return RedirectToAction("Index");
            }

            catch (Exception e)
            {
                TempData[("ErrorMessage")] = $"Encontramos um problema ao cadastrar " +
                    $"seu objetivo! Por favor tente novamente. Detalhes do erro: {e.Message}";
                return View("Index", goalsViewModels);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveIsAtendedInformation(List<Student> students)
        {
            await _assessmentService.AddIsAtendedInformation(students);

            return View();
        }
    }
}
