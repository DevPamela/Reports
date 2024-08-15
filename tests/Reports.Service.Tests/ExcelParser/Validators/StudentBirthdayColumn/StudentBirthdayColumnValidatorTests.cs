using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using NPOI.SS.UserModel;
using Reports.Service.ExcelParser.Validators.StudentBirthdayColumn;
using Reports.Tests.TestHelpers.Files;

namespace Reports.Service.Tests.ExcelParser.Validators.StudentBirthdayColumn;

public class StudentBirthdayColumnValidatorTests
{
    private readonly string[] _filePaths = ["ExcelParser", "Files", "BirthdayColumnValidator"];
    private readonly StudentBirthdayColumnValidator _validator = new();

    [Fact]
    public void Index_ShouldReturn1()
    {
        // Act
        var result = _validator.Index;

        // Assert
        result.Should().Be(1);
    }

    [Fact]
    public void Name_ShouldReturnDataDeNascimento()
    {
        // Act
        var result = _validator.Name;

        // Assert
        result.Should().Be("Data de Nascimento");
    }

    [Fact]
    public void ConvertCellValue_ShouldReturnInvalidType_WhenCellDateValueIsNull()
    {
        // Arrange
        using var helper = new FileStreamHelper([.. _filePaths, "cell-is-not-numeric.xlsx"]);
        using IWorkbook workbook = WorkbookFactory.Create(helper.FileStream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        var expectedError = StudentBirthdayColumnValidationErrors.InvalidType(cell.Address);

        // Act
        var result = _validator.ValidateCellValue(cell);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Fact]
    public void ConvertCellValue_ShouldReturnInvalidType_WhenCellIsNotDateFormatted()
    {
        // Arrange
        using var helper = new FileStreamHelper([.. _filePaths, "cell-is-not-date.xlsx"]);
        using IWorkbook workbook = WorkbookFactory.Create(helper.FileStream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        var expectedError = StudentBirthdayColumnValidationErrors.InvalidType(cell.Address);

        // Act
        var result = _validator.ValidateCellValue(cell);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Fact]
    public void ConvertCellValue_ShouldReturnSuccess_WhenCellValueIsValid()
    {
        // Arrange
        using var helper = new FileStreamHelper([.. _filePaths, "date-is-valid.xlsx"]);
        using IWorkbook workbook = WorkbookFactory.Create(helper.FileStream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        // Act
        var result = _validator.ValidateCellValue(cell);

        // Assert
        result.Should().BeSuccess();
    }
}
