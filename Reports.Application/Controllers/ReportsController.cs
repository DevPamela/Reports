using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Models;

namespace Reports.Application.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportService _service;
        public ReportsController(IReportService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            ViewBag.ValorDoBotao = "Novo Valor do Botão";


            return View();
        }

        [HttpPost]
        public ActionResult CreateReport(DataViewModel data, StudentViewModel model)
        {
            var path = data.Path;
            var report = data.Report;
            var goal = new Goal
            {
                Name = "Adaptação",
                IsSatisfactory = true,
            };
            var goals = new List<Goal>
            {
                goal
            };
            var student1 = new Domain.Models.Student
            {
                Name = "Juliana",
                Goals = goals,
                Teacher = new Teacher
                {
                    Name = "Maria",
                },
                School = new School
                {
                    Name = "Pipoquinha da serra"
                },
                Birthday = "14/09/1988",
                Class = new Class
                {
                    Name = "Inf 4"
                },
                Genre = "F",
            };
            report.Students = new List<Domain.Models.Student>
            {
                student1
            };
            // Aqui você chamaria o método ObterDados com os dados fornecidos pelo usuário
            _service.CreateReport(report, path);
            //var viewModel = new DataViewModel
            ////{
              //  Reports = $"Funcionário: {report}, Path: {path}"
            //};
            // Pode fazer algo com os dados, como exibi-los na View
            //ViewBag.Reports = viewModel.Reports;


            return View(Index);
        }
    }
}
