using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using NPOI.SS.UserModel;
using Reports.Service.ExcelParser.Validators.StudentNameColumn;
using Reports.Tests.TestHelpers;

namespace Reports.Service.Tests.ExcelParser.Validators.StudentNameColumn;

public class StudentNameColumnValidatorTests
{
    private readonly string[] _filePaths = ["ExcelParser", "Files", "NameColumnValidator"];
    private readonly StudentNameColumnValidator _validator = new();

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
        using var helper = new FileStreamHelper([.. _filePaths, "cell-is-not-string.xlsx"]);
        using IWorkbook workbook = WorkbookFactory.Create(helper.FileStream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        var expectedError = StudentNameColumnValidationErrors.InvalidType(cell.Address);

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
        using var helper = new FileStreamHelper([.. _filePaths, "cell-is-whitespace.xlsx"]);
        using IWorkbook workbook = WorkbookFactory.Create(helper.FileStream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        var expectedError = StudentNameColumnValidationErrors.NameIsEmpty(cell.Address);

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
        using var helper = new FileStreamHelper([.. _filePaths, "cell-exceeds-max-length.xlsx"]);
        using IWorkbook workbook = WorkbookFactory.Create(helper.FileStream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        var maxNameLength = 120;
        var expectedError = StudentNameColumnValidationErrors.NameIsTooLong(cell.Address, maxNameLength);

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
        using var helper = new FileStreamHelper([.. _filePaths, "valid-name.xlsx"]);
        using IWorkbook workbook = WorkbookFactory.Create(helper.FileStream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        // Act
        var result = _validator.ValidateCellValue(cell);

        // Assert
        result.Should().BeSuccess();
    }

    [Fact]
    public void ConvertCellValue_ShouldReturnSuccess_WhenNameIsMaxLength()
    {
        // Arrange
        using var helper = new FileStreamHelper([.. _filePaths, "name-is-max-length.xlsx"]);
        using IWorkbook workbook = WorkbookFactory.Create(helper.FileStream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        // Act
        var result = _validator.ValidateCellValue(cell);

        // Assert
        result.Should().BeSuccess();
    }
}
