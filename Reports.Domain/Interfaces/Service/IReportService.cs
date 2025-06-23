using NPOI.XWPF.UserModel;
using Reports.Domain.Entities;

namespace Reports.Domain.Interfaces.Service
{
    public interface IReportService
    {
        /// <summary>
        /// Creates the <see cref="Report">.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="path"></param>
        Task<(byte[] ZipContent, string ZipFileName)> CreateReportAsync(Report report, int teacherId);

    }
}