using FluentResults;
using NPOI.SS.UserModel;
using Reports.Domain.Entities;
using Reports.Service.ExcelParser.Interfaces;

namespace Reports.Service.ExcelParser.Validators.StudentNameColumn;

/// <summary>
/// Represents a validator for the Student Name column in an Excel file.
/// </summary>
public class StudentNameColumnValidator : IColumnValidator, IStudentNameColumn
{
    private const int MaxLength = 120;

    /// <inheritdoc cref="IStudentNameColumn.Index" />
    public int Index => 0;

    /// <inheritdoc />
    public string Name => "Nome";

    /// <inheritdoc />
    public Result ValidateCellValue(ICell cell)
    {
        if (cell.CellType == CellType.Blank)
        {
            return Result.Ok();
        }
        if (cell.CellType is not CellType.String)
        {
            return StudentNameColumnValidationErrors.InvalidType(cell.Address);
        }
        var content = cell.StringCellValue;

        if (string.IsNullOrWhiteSpace(content) && cell.RowIndex < 30)
        {
            return Result.Ok(); 
        }
        
        if (string.IsNullOrWhiteSpace(content))
        {
            return StudentNameColumnValidationErrors.NameIsEmpty(cell.Address);
        }

        if (content.Length > MaxLength)
        {
            return StudentNameColumnValidationErrors.NameIsTooLong(cell.Address, MaxLength);
        }

        return Result.Ok();
    }
}
