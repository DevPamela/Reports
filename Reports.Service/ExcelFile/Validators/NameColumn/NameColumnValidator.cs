using FluentResults;
using NPOI.SS.UserModel;
using Reports.Service.ExcelFile.Interfaces;

namespace Reports.Service.ExcelFile.Validators.NameColumn;

public class NameColumnValidator : IColumnValidator, INameColumnInfo
{
    private const int MaxLength = 120;

    public int Index => 0;
    public string Name => "Nome";

    public Result ValidateCellValue(ICell cell)
    {
        if (cell.CellType is not CellType.String)
        {
            return NameColumnValidationErrors.InvalidType(cell.Address);
        }

        var content = cell.StringCellValue;

        if (string.IsNullOrWhiteSpace(content))
        {
            return NameColumnValidationErrors.NameIsEmpty(cell.Address);
        }

        if (content.Length > MaxLength)
        {
            return NameColumnValidationErrors.NameIsTooLong(cell.Address, MaxLength);
        }

        return Result.Ok();
    }
}
