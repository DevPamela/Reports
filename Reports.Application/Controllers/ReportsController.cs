using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Interfaces;
using Reports.Domain.Interfaces.Service;

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
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult>CreateReport(DataViewModel data)
        {
            try
            {
                var report = data.Report;

                await _service.CreateReportAsync(report);

                return View("Success");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
            
        }

    }
}
