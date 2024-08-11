namespace Reports.Service.ExcelParser.Interfaces;

/// <summary>
/// Represents the interface for a Student Birthday column in an Excel file.
/// </summary>
public interface IStudentBirthdayColumn
{
    /// <summary>
    /// Gets the index of the column.
    /// </summary>
    public int Index { get; }
}
