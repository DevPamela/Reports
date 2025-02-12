using FluentResults;
using NPOI.SS.UserModel;
using Reports.Service.ExcelParser.Interfaces;

namespace Reports.Service.ExcelParser.Validators.StudentGenderColumn;


/// <summary>
/// Represents a validator for the Student Gender column in an Excel file.
/// </summary>
public class StudentGenderColumnValidator : IColumnValidator, IStudentGenderColumn
{
    /// <inheritdoc cref="IStudentGenderColumn.Index" />
    public int Index => 2;

    /// <inheritdoc />
    public string Name => "Gênero";

    /// <inheritdoc />
    public Result ValidateCellValue(ICell cell)
    {
        if (cell.CellType == CellType.Blank)
        {
            return Result.Ok(); 
        }
        if (cell.CellType is not CellType.String)
        {
            return StudentGenderColumnValidationErrors.InvalidType(cell.Address);
        }

        var content = cell.StringCellValue;

        if (string.IsNullOrWhiteSpace(content))
        {
            return StudentGenderColumnValidationErrors.GenderIsEmpty(cell.Address);
        }

        if (!content.Equals("Feminino", StringComparison.OrdinalIgnoreCase)
            && !content.Equals("Masculino", StringComparison.OrdinalIgnoreCase))
        {
            return StudentGenderColumnValidationErrors.GenderIsInvalid(cell.Address);
        }

        return Result.Ok();
    }
}
