using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Interfaces.Service;

namespace Reports.Application.Controllers
{
    [Authorize]
    public class ReportsController : BaseController
    {
        private readonly IReportService _service;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IReportService service, ILogger<ReportsController> logger): base(logger)
        {
            _service = service;
            _logger = logger;
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
        public async Task<ActionResult> CreateReport(DataViewModel data)
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
