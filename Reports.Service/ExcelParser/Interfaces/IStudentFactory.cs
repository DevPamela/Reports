using NPOI.SS.UserModel;
using Reports.Domain.Models;

namespace Reports.Service.ExcelParser.Interfaces;

/// <summary>
/// Represents the interface for a factory for creating Student objects.
/// </summary>
public interface IStudentFactory
{
    /// <summary>
    /// Creates a Student object based on the provided IRow.
    /// </summary>
    /// <param name="row">The IRow containing the student data.</param>
    /// <returns>The created Student object.</returns>
    Student CreateStudent(IRow row);
}
