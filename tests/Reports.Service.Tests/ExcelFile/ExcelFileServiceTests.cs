using FluentAssertions;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NSubstitute;
using Reports.Domain.Models;
using Reports.Service.ExcelFile;
using Reports.Service.ExcelFile.Interfaces;
using Reports.Service.Tests.ExcelFile.Helpers;

namespace Reports.Service.Tests.ExcelFile;

public class ExcelFileServiceTests
{
    private readonly ExcelFileService _service;
    private readonly IColumnValidator _mockColumnValidator;
    private readonly IStudentFactory _mockStudentFactory;

    public ExcelFileServiceTests()
    {
        _mockColumnValidator = Substitute.For<IColumnValidator>();
        _mockColumnValidator.Index.Returns(0);
        _mockColumnValidator.Name.Returns("Nome");

        _mockStudentFactory = Substitute.For<IStudentFactory>();

        _service = new ExcelFileService([_mockColumnValidator], _mockStudentFactory);
    }

    [Fact]
    public void ReadStudentsFromFile_ShouldReturnHeaderIsNull_WhenHeaderRowIsNull()
    {
        // Arrange
        using var stream = new FileStreamHelper("empty-sheet").FileStream;

        var expectedError = ExcelFileValidationErrors.HeaderIsNull();

        // Act
        var result = _service.ReadStudentsFromFile(stream);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Theory]
    [InlineData("null-header")]
    [InlineData("header-is-not-string")]
    [InlineData("wrong-header-name")]
    public void ReadStudentsFromFile_ShouldReturnColumnNotFound_WhenHeaderCellIsInvalid(string fileName)
    {
        // Arrange
        using var stream = new FileStreamHelper(fileName).FileStream;

        var expectedError = ExcelFileValidationErrors.ColumnNotFound("Nome", "A");

        // Act
        var result = _service.ReadStudentsFromFile(stream);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Fact]
    public void ReadStudentsFromFile_ShouldReturnRowIsNull_WhenDataRowIsNull()
    {
        // Arrange
        using var stream = new FileStreamHelper("empty-row").FileStream;
        _mockColumnValidator.ValidateCellValue(Arg.Any<ICell>()).Returns(Result.Ok());

        var expectedError = ExcelFileValidationErrors.RowIsNull(2);

        // Act
        var result = _service.ReadStudentsFromFile(stream);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Fact]
    public void ReadStudentsFromFile_ShouldReturnCellIsNull_WhenDataCellIsNull()
    {
        // Arrange
        using var stream = new FileStreamHelper("empty-cell").FileStream;

        var expectedError = ExcelFileValidationErrors.CellIsNull("Nome", new CellAddress(1, 0));

        // Act
        var result = _service.ReadStudentsFromFile(stream);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Fact]
    public void ReadStudentsFromFile_ShouldReturnStudentDataNotFound_WhenNoStudentsAreProcessed()
    {
        // Arrange
        using var stream = new FileStreamHelper("no-students").FileStream;

        var expectedError = ExcelFileValidationErrors.StudentDataNotFound();

        // Act
        var result = _service.ReadStudentsFromFile(stream);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Fact]
    public void ReadStudentsFromFile_ShouldReturnErrorThresholdReached_WhenErrorsReachThreshold()
    {
        // Arrange
        using var stream = new FileStreamHelper("error-threshold").FileStream;
        _mockColumnValidator.ValidateCellValue(Arg.Any<ICell>()).Returns(Result.Fail("some error"));

        var expectedError = ExcelFileValidationErrors.ErrorThresholdReached();

        // Act
        var result = _service.ReadStudentsFromFile(stream);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(expectedError.Message);
    }

    [Fact]
    public void ReadStudentsFromFile_ShouldReturnStudent_WhenDataIsValid()
    {
        // Arrange
        using var stream = new FileStreamHelper("valid-data").FileStream;
        _mockColumnValidator.ValidateCellValue(Arg.Any<ICell>()).Returns(Result.Ok());

        var expectedName = "John Doe";
        _mockStudentFactory.CreateStudent(Arg.Any<IRow>()).Returns(new Student { Name = expectedName });

        // Act
        var result = _service.ReadStudentsFromFile(stream);

        // Assert
        result.Should().BeSuccess();
        result.Value.Should().HaveCount(1);
        result.Value[0].Name.Should().Be(expectedName);
    }
}
