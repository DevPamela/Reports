using FluentResults;
using NPOI.SS.UserModel;
using Reports.Service.ExcelParser.Interfaces;

namespace Reports.Service.ExcelParser.Validators.StudentBirthdayColumn;

/// <summary>
/// Represents a validator for the Student Birthday column in an Excel file.
/// </summary>
public class StudentBirthdayColumnValidator : IColumnValidator, IStudentBirthdayColumn
{
    /// <inheritdoc cref="IStudentBirthdayColumn.Index" />
    public int Index => 1;

    /// <inheritdoc />
    public string Name => "Data de Nascimento";

    /// <inheritdoc />
    public Result ValidateCellValue(ICell cell)
    {
        if (cell.DateCellValue is null
            || !DateUtil.IsCellDateFormatted(cell))
        {
            return StudentBirthdayColumnValidationErrors.InvalidType(cell.Address);
        }

        return Result.Ok();
    }
}
