
using Reports.Domain.Models;

namespace Reports.Domain.Interfaces.Service
{
    public interface IReportService
    {
        /// <summary>
        /// Creates the report.
        /// </summary>
        /// <param name="Report"></param>
        /// <param name="path"></param>
        void CreateReport(Report Report, string path);
    }
}