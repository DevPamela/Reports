namespace Reports.Service.ExcelParser.Interfaces;

/// <summary>
/// Represents the interface for a Student Birthdate column in an Excel file.
/// </summary>
public interface IStudentBirthdateColumn
{
    /// <summary>
    /// Gets the index of the column.
    /// </summary>
    public int Index { get; }
}
