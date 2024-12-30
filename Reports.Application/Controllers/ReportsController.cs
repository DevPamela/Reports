using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Interfaces.Service;

namespace Reports.Application.Controllers
{
    [Authorize]
    public class ReportsController : BaseController<ReportsController>
    {
        private readonly IReportService _service;

        public ReportsController(IReportService service, ILogger<ReportsController> logger) : base(logger)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            Logger.LogInformation("Reports Index page accessed.");
            return View();
        }

        public ActionResult Error()
        {
            Logger.LogWarning("Reports Error page accessed.");
            return View();
        }

        public ActionResult Success()
        {
            Logger.LogInformation("Reports Success page accessed.");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateReport(DataViewModel data)
        {
            try
            {
                Logger.LogInformation("Starting report creation process with data: {DataModel}", data);

                var report = data.Report;
                await _service.CreateReportAsync(report);

                Logger.LogInformation("Report created successfully.");
                return View("Success");
            }
            catch (Exception ex)
            {
                var userId = GetUserIdFromSession();
                Logger.LogError(ex, "An error occurred for User {UserId} while creating a report with data: {DataModel}", userId, data);
                return View("Error", ex);
            }
        }
    }
}
