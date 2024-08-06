using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using NPOI.SS.UserModel;
using Reports.Service.ExcelFile.Validators.NameColumn;
using Reports.Service.Tests.ExcelFile.Helpers;

namespace Reports.Service.Tests.ExcelFile.Validators.NameColumn;

public class NameColumnValidatorTests
{
    private readonly NameColumnValidator _validator;

    public NameColumnValidatorTests()
    {
        _validator = new NameColumnValidator();
    }

    [Fact]
    public void Index_ShouldReturn0()
    {
        // Act
        var result = _validator.Index;

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void Name_ShouldReturnNome()
    {
        // Act
        var result = _validator.Name;

        // Assert
        result.Should().Be("Nome");
    }

    [Fact]
    public void ConvertCellValue_ShouldReturnInvalidType_WhenCellTypeIsNotString()
    {
        // Arrange
        using var stream = new FileStreamHelper("NameColumnValidator", "cell-is-not-string").FileStream;
        using IWorkbook workbook = WorkbookFactory.Create(stream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        var expectedError = NameColumnValidationErrors.InvalidType(cell.Address);

        // Act
        var result = _validator.ValidateCellValue(cell);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Fact]
    public void ConvertCellValue_ShouldReturnNameIsEmpty_WhenCellValueIsNullOrWhiteSpace()
    {
        // Arrange
        using var stream = new FileStreamHelper("NameColumnValidator", "cell-is-whitespace").FileStream;
        using IWorkbook workbook = WorkbookFactory.Create(stream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        var expectedError = NameColumnValidationErrors.NameIsEmpty(cell.Address);

        // Act
        var result = _validator.ValidateCellValue(cell);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Fact]
    public void ConvertCellValue_ShouldReturnNameIsTooLong_WhenCellValueExceedsMaxLength()
    {
        // Arrange
        using var stream = new FileStreamHelper("NameColumnValidator", "cell-exceeds-max-length").FileStream;
        using IWorkbook workbook = WorkbookFactory.Create(stream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        var maxNameLength = 120;
        var expectedError = NameColumnValidationErrors.NameIsTooLong(cell.Address, maxNameLength);

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
        using var stream = new FileStreamHelper("NameColumnValidator", "valid-name").FileStream;
        using IWorkbook workbook = WorkbookFactory.Create(stream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        // Act
        var result = _validator.ValidateCellValue(cell);

        // Assert
        result.Should().BeSuccess();
    }

    [Fact]
    public void ConvertCellValue_ShouldReturndSuccess_WhenNameIsMaxLength()
    {
        // Arrange
        using var stream = new FileStreamHelper("NameColumnValidator", "name-is-max-length").FileStream;
        using IWorkbook workbook = WorkbookFactory.Create(stream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        // Act
        var result = _validator.ValidateCellValue(cell);

        // Assert
        result.Should().BeSuccess();
    }
}
