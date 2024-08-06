using FluentResults;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Reports.Domain.Models;
using Reports.Service.ExcelFile.Interfaces;

namespace Reports.Service.ExcelFile;

public class ExcelFileService
{
    // TODO move constants to a separate class/config file
    private const int StudentProcessingErrorThreshold = 10;

    private readonly List<IColumnValidator> _validators;
    private readonly IStudentFactory _studentFactory;

    public ExcelFileService(
        IEnumerable<IColumnValidator> validators,
        IStudentFactory studentFactory)
    {
        _validators = validators.ToList();
        _studentFactory = studentFactory;
    }

    public Result<List<Student>> ReadStudentsFromFile(Stream stream)
    {
        using IWorkbook workbook = WorkbookFactory.Create(stream);
        return ProcessSheet(workbook.GetSheetAt(0));
    }

    private Result<List<Student>> ProcessSheet(ISheet sheet)
    {
        var validateHeaderResult = ValidateHeader(sheet.GetRow(0));

        if (validateHeaderResult.IsFailed)
        {
            return validateHeaderResult;
        }

        var processRowsResult = ProcessStudentRows(sheet);

        if (processRowsResult.IsFailed)
        {
            return processRowsResult;
        }

        if (processRowsResult.Value.Count == 0)
        {
            return ExcelFileValidationErrors.StudentDataNotFound();
        }

        return processRowsResult.Value;
    }

    private Result ValidateHeader(IRow headerRow)
    {
        if (headerRow is null)
        {
            return ExcelFileValidationErrors.HeaderIsNull();
        }

        var result = new Result();

        foreach (var validator in _validators)
        {
            ICell headerCell = headerRow.GetCell(validator.Index);

            if (headerCell?.CellType is not CellType.String
                || !headerCell.StringCellValue.Equals(validator.Name, StringComparison.OrdinalIgnoreCase))
            {
                result.WithError(ExcelFileValidationErrors.ColumnNotFound(
                    validator.Name, CellReference.ConvertNumToColString(validator.Index)));
            }
        }

        return result;
    }

    private Result<List<Student>> ProcessStudentRows(ISheet sheet)
    {
        var result = new Result();
        var students = new List<Student>(sheet.LastRowNum);

        for (var i = 1; i <= sheet.LastRowNum; i++)
        {
            if (result.Errors.Count >= StudentProcessingErrorThreshold)
            {
                return result.WithError(
                    ExcelFileValidationErrors.ErrorThresholdReached());
            }

            var processRowResult = ProcessRow(sheet.GetRow(i), i);

            if (processRowResult.IsFailed)
            {
                result.WithErrors(processRowResult.Errors);
                continue;
            }

            students.Add(processRowResult.Value);
        }

        if (result.IsFailed)
        {
            return result;
        }

        return students;
    }

    private Result<Student> ProcessRow(IRow row, int rowIndex)
    {
        if (row is null)
        {
            return ExcelFileValidationErrors.RowIsNull(rowIndex + 1);
        }

        var result = new Result();

        foreach (var validator in _validators)
        {
            ICell cell = row.GetCell(validator.Index);

            if (cell is null)
            {
                result.WithError(ExcelFileValidationErrors.CellIsNull(
                    validator.Name, new CellAddress(row.RowNum, validator.Index)));
                continue;
            }

            var validateCellResult = validator.ValidateCellValue(cell);

            if (validateCellResult.IsFailed)
            {
                result.WithErrors(validateCellResult.Errors);
            }
        }

        if (result.IsFailed)
        {
            return result;
        }

        return _studentFactory.CreateStudent(row);
    }
}
