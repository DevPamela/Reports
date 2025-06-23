using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reports.Application.Models;
using Reports.Domain.Entities;
using Reports.Domain.Interfaces.Service;
using Reports.Service;
using System.IO.Compression;

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

     
        [HttpPost]
        public async Task<ActionResult> CreateReport(DataViewModel data)
        {
            Logger.LogInformation("Starting report creation process with data: {DataModel}", data);

            try
            {
                var userId = GetUserIdFromSession();
                var report = data.Report;
                var (zipContent, zipFileName) = await _service.CreateReportAsync(report, int.Parse(userId));

              
                var tempFile = Path.GetTempFileName();
                await System.IO.File.WriteAllBytesAsync(tempFile, zipContent);

                HttpContext.Session.SetString("TempZipPath", tempFile);
                HttpContext.Session.SetString("ZipFileName", zipFileName);

                _ = Task.Run(async () =>
                {
                    await Task.Delay(30 * 60 * 1000); 
                    if (System.IO.File.Exists(tempFile))
                        System.IO.File.Delete(tempFile);
                });

                return View("Success");

            }
            catch (Exception ex)
            {
                var userId = GetUserIdFromSession();
                Logger.LogError(ex, "An error occurred for User {UserId} while creating a report with data: {DataModel}", userId, data);
                return View("Error", ex);
            }
        }
        [HttpGet]
        public async Task<IActionResult> DownloadReports()
        {
            try
            {
                var tempFile = HttpContext.Session.GetString("TempZipPath");
                var fileName = HttpContext.Session.GetString("ZipFileName") ?? "Relatorios.zip";

                if (string.IsNullOrEmpty(tempFile) || !System.IO.File.Exists(tempFile))
                    return NotFound();

                var zipData = await System.IO.File.ReadAllBytesAsync(tempFile);

                System.IO.File.Delete(tempFile);
                HttpContext.Session.Remove("TempZipPath");
                HttpContext.Session.Remove("ZipFileName");

                return File(
                    fileContents: zipData,
                    contentType: "application/zip",
                    fileDownloadName: fileName
                );
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Erro ao baixar relatórios");
                return StatusCode(500);
            }
        }
    }
}
