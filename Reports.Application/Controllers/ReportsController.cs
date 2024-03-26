using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Interfaces;

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
            ViewBag.ValorDoBotao = "Enviar";
            return View();
        }

        [HttpPost]
        public ActionResult CreateReport(DataViewModel data)
        {
            var path = data.Path;
            var report = data.Report;
            
            _service.CreateReport(report, path);

            //TODO retornar os relatórios
            return View();
        }

    }
}
