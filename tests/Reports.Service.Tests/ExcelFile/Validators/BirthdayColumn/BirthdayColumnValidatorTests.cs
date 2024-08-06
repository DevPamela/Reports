using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using NPOI.SS.UserModel;
using Reports.Service.ExcelFile.Validators.BirthdayColumn;
using Reports.Service.Tests.ExcelFile.Helpers;

namespace Reports.Service.Tests.ExcelFile.Validators.BirthdayColumn;

public class BirthdayColumnValidatorTests
{
    private readonly BirthdayColumnValidator _validator;

    public BirthdayColumnValidatorTests()
    {
        _validator = new BirthdayColumnValidator();
    }

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
        using var stream = new FileStreamHelper("BirthdayColumnValidator", "cell-is-not-numeric").FileStream;
        using IWorkbook workbook = WorkbookFactory.Create(stream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        var expectedError = BirthdayColumnValidationErrors.InvalidType(cell.Address);

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
        using var stream = new FileStreamHelper("BirthdayColumnValidator", "cell-is-not-date").FileStream;
        using IWorkbook workbook = WorkbookFactory.Create(stream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        var expectedError = BirthdayColumnValidationErrors.InvalidType(cell.Address);

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
        using var stream = new FileStreamHelper("BirthdayColumnValidator", "date-is-valid").FileStream;
        using IWorkbook workbook = WorkbookFactory.Create(stream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        // Act
        var result = _validator.ValidateCellValue(cell);

        // Assert
        result.Should().BeSuccess();
    }
}
