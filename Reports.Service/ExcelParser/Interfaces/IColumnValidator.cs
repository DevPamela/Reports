using FluentResults;
using NPOI.SS.UserModel;

namespace Reports.Service.ExcelParser.Interfaces;

/// <summary>
/// Represents the interface for a column validator for Excel parsing.
/// </summary>
public interface IColumnValidator
{
    /// <summary>
    /// Gets the index of the column.
    /// </summary>
    int Index { get; }

    /// <summary>
    /// Gets the name of the column.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Validates the cell value.
    /// </summary>
    /// <param name="cell">The cell to validate.</param>
    /// <returns>The result of the validation.</returns>
    public Result ValidateCellValue(ICell cell);
}
