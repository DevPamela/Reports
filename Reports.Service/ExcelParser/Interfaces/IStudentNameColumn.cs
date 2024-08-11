namespace Reports.Service.ExcelParser.Interfaces;

/// <summary>
/// Represents the interface for a Student Name column in an Excel file.
/// </summary>
public interface IStudentNameColumn
{
    /// <summary>
    /// Gets the index of the column.
    /// </summary>
    int Index { get; }
}
