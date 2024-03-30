using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Constants;
using Reports.Domain.Interfaces;
using Reports.Infrastructure;

namespace Reports.Application.Controllers
{
    public class GoalsController : Controller
    {
        private readonly IGoalService _goalService;
        public GoalsController(IGoalService goalService)
        {
            _goalService = goalService;
        }

        public IActionResult Index()
        {
            var viewModel = new List<GoalsViewModel> { new GoalsViewModel(), new GoalsViewModel(), new GoalsViewModel() };
            ViewBag.ValorDoBotao = "Enviar";
            return View(viewModel);
        }

        public ActionResult SelectGoalsForReport()
        {
            var goals = _goalService.GetGoalsList();
            List<string> goalsName = goals.Select(g => g.Name).ToList();

            //List<string> objetivos = new List<string>() { "Adaptação", "interação", "roda De Conversa", "Coordenação Motora", "Resolucao De Conflitos" };

            ViewBag.Objetivos = goalsName;
            return View(goalsName);
        }

        [HttpPost]
        public ActionResult SaveStudentGoalsResults(List<string> goals)
        {
            var students = _goalService.SaveStudentsGoals(goals);
            return View(students);

        }

        [HttpPost]
        public ActionResult AddGoal(List<GoalsViewModel> goalsViewModels)
        {
            var goalName = goalsViewModels[0].Name;

            for (int i = 0; i < goalsViewModels.Count; i++)
            {
                var collectionName = goalsViewModels[i].collectionName;
                var description = goalsViewModels[i].Description;

                _goalService.AddGoalInColletion(collectionName, goalName.ToString(), description);
            }

            return View();
        }
    }
}
