using FluentResults;
using NPOI.SS.UserModel;
using Reports.Service.ExcelFile.Interfaces;

namespace Reports.Service.ExcelFile.Validators.BirthdayColumn;

public class BirthdayColumnValidator : IColumnValidator, IBirthdayColumnInfo
{
    public int Index => 1;
    public string Name => "Data de Nascimento";

    public Result ValidateCellValue(ICell cell)
    {
        if (cell.DateCellValue is null
            || !DateUtil.IsCellDateFormatted(cell))
        {
            return BirthdayColumnValidationErrors.InvalidType(cell.Address);
        }

        return Result.Ok();
    }
}
