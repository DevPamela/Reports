using FluentResults;
using NPOI.SS.UserModel;

namespace Reports.Service.ExcelFile.Interfaces;

public interface IColumnValidator
{
    int Index { get; }
    string Name { get; }

    public Result ValidateCellValue(ICell cell);
}
