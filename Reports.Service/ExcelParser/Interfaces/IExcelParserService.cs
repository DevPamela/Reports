using FluentResults;
using Reports.Domain.Entities;

namespace Reports.Service.ExcelParser.Interfaces;

/// <summary>
/// Represents the interface for parsing Excel files and extracting student data.
/// </summary>
public interface IExcelParserService
{
    /// <summary>
    /// Reads the students from the specified Excel file stream.
    /// </summary>
    /// <param name="stream">The stream of the Excel file.</param>
    /// <returns>A result containing the list of students read from the file.</returns>
    Result<List<Student>> ReadStudentsFromFile(Stream stream);
}
