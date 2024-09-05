using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using NPOI.SS.UserModel;
using Reports.Service.ExcelParser.Validators.StudentGenderColumn;
using Reports.Tests.TestHelpers.Files;

namespace Reports.Service.Tests.ExcelParser.Validators.StudentGenderColumn;

public class StudentGenderColumnValidatorTests
{
    private readonly string[] _filePaths = ["ExcelParser", "Files", "GenderColumnValidator"];
    private readonly StudentGenderColumnValidator _validator = new();

    [Fact]
    public void Index_ShouldReturn2()
    {
        // Act
        var result = _validator.Index;

        // Assert
        result.Should().Be(2);
    }

    [Fact]
    public void Name_ShouldReturnGenero()
    {
        // Act
        var result = _validator.Name;

        // Assert
        result.Should().Be("Gênero");
    }

    [Fact]
    public void ValidateCellValue_ShouldReturnInvalidType_WhenCellTypeIsNotString()
    {
        // Arrange
        using var helper = new FileStreamHelper([.. _filePaths, "cell-is-not-string.xlsx"]);
        using IWorkbook workbook = WorkbookFactory.Create(helper.FileStream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        var expectedError = StudentGenderColumnValidationErrors.InvalidType(cell.Address);

        // Act
        var result = _validator.ValidateCellValue(cell);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Fact]
    public void ValidateCellValue_ShouldReturnGenderIsEmpty_WhenCellValueIsNullOrWhiteSpace()
    {
        // Arrange
        using var helper = new FileStreamHelper([.. _filePaths, "cell-is-whitespace.xlsx"]);
        using IWorkbook workbook = WorkbookFactory.Create(helper.FileStream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        var expectedError = StudentGenderColumnValidationErrors.GenderIsEmpty(cell.Address);

        // Act
        var result = _validator.ValidateCellValue(cell);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Fact]
    public void ValidateCellValue_ShouldReturnGenderIsInvalid_WhenCellValueIsNotFemininoOrMasculino()
    {
        // Arrange
        using var helper = new FileStreamHelper([.. _filePaths, "gender-is-invalid.xlsx"]);
        using IWorkbook workbook = WorkbookFactory.Create(helper.FileStream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        var expectedError = StudentGenderColumnValidationErrors.GenderIsInvalid(cell.Address);

        // Act
        var result = _validator.ValidateCellValue(cell);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Theory]
    [InlineData("feminino")]
    [InlineData("feminino-lower")]
    [InlineData("masculino")]
    [InlineData("masculino-lower")]
    public void ValidateCellValue_ShouldReturnSuccess_WhenCellValueIsValid(string filename)
    {
        // Arrange
        using var helper = new FileStreamHelper([.. _filePaths, $"{filename}.xlsx"]);
        using IWorkbook workbook = WorkbookFactory.Create(helper.FileStream);
        ICell cell = workbook.GetSheetAt(0).GetRow(0).GetCell(0);

        // Act
        var result = _validator.ValidateCellValue(cell);

        // Assert
        result.Should().BeSuccess();
    }
}
