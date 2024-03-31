using Reports.Domain.Models;

namespace Reports.Domain.Interfaces
{
    public interface IReportService
    {
        /// <summary>
        /// Creates the report.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="path"></param>
        Task CreateReportAsync(Report report, string path);
    }
}