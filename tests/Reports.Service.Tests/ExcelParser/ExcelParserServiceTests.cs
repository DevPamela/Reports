using FluentAssertions;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NSubstitute;
using Reports.Domain.Models;
using Reports.Service.ExcelParser;
using Reports.Service.ExcelParser.Interfaces;
using Reports.Tests.TestHelpers.Files;

namespace Reports.Service.Tests.ExcelParser;

public class ExcelParserServiceTests
{
    private readonly string[] _filePaths = ["ExcelParser", "Files"];
    private readonly IColumnValidator _mockColumnValidator = Substitute.For<IColumnValidator>();
    private readonly IStudentFactory _mockStudentFactory = Substitute.For<IStudentFactory>();
    private readonly ExcelParserService _service;

    public ExcelParserServiceTests()
    {
        _mockColumnValidator.Index.Returns(0);
        _mockColumnValidator.Name.Returns("Nome");

        _service = new ExcelParserService([_mockColumnValidator], _mockStudentFactory);
    }

    [Fact]
    public void ReadStudentsFromFile_ShouldReturnHeaderIsNull_WhenHeaderRowIsNull()
    {
        // Arrange
        using var helper = new FileStreamHelper([.. _filePaths, "empty-sheet.xlsx"]);

        var expectedError = ExcelParserValidationErrors.HeaderIsNull();

        // Act
        var result = _service.ReadStudentsFromFile(helper.FileStream);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Theory]
    [InlineData("null-header.xlsx")]
    [InlineData("header-is-not-string.xlsx")]
    [InlineData("wrong-header-name.xlsx")]
    public void ReadStudentsFromFile_ShouldReturnColumnNotFound_WhenHeaderCellIsInvalid(string fileName)
    {
        // Arrange
        using var helper = new FileStreamHelper([.. _filePaths, fileName]);

        var expectedError = ExcelParserValidationErrors.ColumnNotFound("Nome", "A");

        // Act
        var result = _service.ReadStudentsFromFile(helper.FileStream);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Fact]
    public void ReadStudentsFromFile_ShouldReturnRowIsNull_WhenDataRowIsNull()
    {
        // Arrange
        using var helper = new FileStreamHelper([.. _filePaths, "empty-row.xlsx"]);
        _mockColumnValidator.ValidateCellValue(Arg.Any<ICell>()).Returns(Result.Ok());

        var expectedError = ExcelParserValidationErrors.RowIsNull(2);

        // Act
        var result = _service.ReadStudentsFromFile(helper.FileStream);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Fact]
    public void ReadStudentsFromFile_ShouldReturnCellIsNull_WhenDataCellIsNull()
    {
        // Arrange
        using var helper = new FileStreamHelper([.. _filePaths, "empty-cell.xlsx"]);

        var expectedError = ExcelParserValidationErrors.CellIsNull("Nome", new CellAddress(1, 0));

        // Act
        var result = _service.ReadStudentsFromFile(helper.FileStream);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Fact]
    public void ReadStudentsFromFile_ShouldReturnStudentDataNotFound_WhenNoStudentsAreProcessed()
    {
        // Arrange
        using var helper = new FileStreamHelper([.. _filePaths, "no-students.xlsx"]);

        var expectedError = ExcelParserValidationErrors.StudentDataNotFound();

        // Act
        var result = _service.ReadStudentsFromFile(helper.FileStream);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Fact]
    public void ReadStudentsFromFile_ShouldReturnErrorThresholdReached_WhenErrorsReachThreshold()
    {
        // Arrange
        using var helper = new FileStreamHelper([.. _filePaths, "error-threshold.xlsx"]);
        _mockColumnValidator.ValidateCellValue(Arg.Any<ICell>()).Returns(Result.Fail("some error"));

        var expectedError = ExcelParserValidationErrors.ErrorThresholdReached();

        // Act
        var result = _service.ReadStudentsFromFile(helper.FileStream);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Fact]
    public void ReadStudentsFromFile_ShouldReturnStudent_WhenDataIsValid()
    {
        // Arrange
        using var helper = new FileStreamHelper([.. _filePaths, "valid-data.xlsx"]);
        _mockColumnValidator.ValidateCellValue(Arg.Any<ICell>()).Returns(Result.Ok());

        var expectedName = "John Doe";
        _mockStudentFactory.CreateStudent(Arg.Any<IRow>()).Returns(new Student { Name = expectedName });

        // Act
        var result = _service.ReadStudentsFromFile(helper.FileStream);

        // Assert
        result.Should().BeSuccess();
        result.Value.Should().HaveCount(1);
        result.Value[0].Name.Should().Be(expectedName);
    }
}
