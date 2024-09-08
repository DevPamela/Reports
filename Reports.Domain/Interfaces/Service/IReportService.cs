using Reports.Domain.Models;

namespace Reports.Domain.Interfaces.Service
{
    public interface IReportService
    {
        /// <summary>
        /// Creates the <see cref="Report">.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="path"></param>
        Task CreateReportAsync(Report report);

        /// <summary>
        /// Gets the <see cref="Report"> by Id.
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        Task GetReportById(Report report);
    }
}