using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Entities;
using Reports.Domain.Interfaces.Service;

namespace Reports.Application.Controllers
{
    [Authorize]
    public class GoalsController : Controller
    {
        private readonly IGoalService _goalService;
        private readonly IAssessmentService _assessmentService;
        private readonly ITeacherService _teacherService;
        public GoalsController(IGoalService goalService, IAssessmentService assessmentService, ITeacherService teacherService)
        {
            _goalService = goalService;
            _assessmentService = assessmentService;
            _teacherService = teacherService;
        }

        public IActionResult Index()
        {
            var viewModel = new List<GoalsViewModel> { new GoalsViewModel(), new GoalsViewModel(), new GoalsViewModel() };
            ViewBag.ValorDoBotao = "Enviar";
            return View(viewModel);
        }

        public async Task<ActionResult> SelectGoalsForReport() 
        {
            var user = User.FindFirst("UserId")?.Value;

            var teacher = await _teacherService.GetByUserIdAsync(int.Parse(user));
            var goals = await _goalService.GetGoalsListAsyncByTeacherId(teacher.Id);

            var goalsName = goals.Select(g => g.Name).ToList();

            ViewBag.Objetivos = goalsName;
            return View(goalsName);
        }


        [HttpPost]
        public async Task<ActionResult> SaveStudentGoalsResults(List<string> goals)
        {
            var students = await _goalService.SaveStudentsGoalsAsync(goals);

            return View(students);

        }
       
        [HttpPost]
        public async Task<ActionResult> AddGoal(List<GoalsViewModel> goalsViewModels)
        {
            try
            {
                var goalName = goalsViewModels[0].Name;
                var user = User.FindFirst("UserId")?.Value;

                var teacher = await _teacherService.GetByUserIdAsync(int.Parse(user));
                foreach (var goalViewModel in goalsViewModels)
                {
                    var collectionName = goalViewModel.collectionName;
                    var description = goalViewModel.Description;
                    var isGirlsGoal = goalViewModel.IsGirl;

                    // Chama o serviço para adicionar a meta
                    await _goalService.AddGoalsAsync(collectionName, goalName, description, isGirlsGoal, teacher.Id);
                }
                

                TempData["SuccessMessage"] = "Cadastro realizado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = $"Encontramos um problema ao cadastrar seu objetivo! Erro: {e.Message}";
                return View("Index", goalsViewModels);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveIsAtendedInformation(List<Student> students)
        {
            await _assessmentService.AddIsAtendedInformation(students);

            return View();
        }
    }
}