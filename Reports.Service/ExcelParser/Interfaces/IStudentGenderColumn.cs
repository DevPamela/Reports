namespace Reports.Service.ExcelParser.Interfaces;

/// <summary>
/// Represents an interface for a Student Gender column in an Excel file.
/// </summary>
public interface IStudentGenderColumn
{
    /// <summary>
    /// Gets the index of the column.
    /// </summary>
    int Index { get; }
}
