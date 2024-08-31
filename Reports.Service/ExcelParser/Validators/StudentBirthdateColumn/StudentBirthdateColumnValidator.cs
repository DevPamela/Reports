using FluentResults;
using NPOI.SS.UserModel;
using Reports.Service.ExcelParser.Interfaces;

namespace Reports.Service.ExcelParser.Validators.StudentBirthdateColumn;

/// <summary>
/// Represents a validator for the Student Birthdate column in an Excel file.
/// </summary>
public class StudentBirthdateColumnValidator : IColumnValidator, IStudentBirthdateColumn
{
    /// <inheritdoc cref="IStudentBirthdateColumn.Index" />
    public int Index => 1;

    /// <inheritdoc />
    public string Name => "Data de Nascimento";

    /// <inheritdoc />
    public Result ValidateCellValue(ICell cell)
    {
        if (cell.DateCellValue is null
            || !DateUtil.IsCellDateFormatted(cell))
        {
            return StudentBirthdateColumnValidationErrors.InvalidType(cell.Address);
        }

        return Result.Ok();
    }
}
