using FluentAssertions;
using NPOI.SS.UserModel;
using NSubstitute;
using Reports.Domain.Enums;
using Reports.Service.ExcelParser.Factories;
using Reports.Service.ExcelParser.Interfaces;
using Reports.Tests.TestHelpers.Files;

namespace Reports.Service.Tests.ExcelParser.Factories;

public class StudentFactoryTests
{
    private readonly IStudentNameColumn _mockNameColumn = Substitute.For<IStudentNameColumn>();
    private readonly IStudentBirthdateColumn _mockBirthdateColumn = Substitute.For<IStudentBirthdateColumn>();
    private readonly IStudentGenderColumn _mockGenderColumn = Substitute.For<IStudentGenderColumn>();
    private readonly StudentFactory _studentFactory;

    public StudentFactoryTests()
    {
        _mockNameColumn.Index.Returns(0);
        _mockBirthdateColumn.Index.Returns(1);
        _mockGenderColumn.Index.Returns(2);
        _studentFactory = new StudentFactory(_mockNameColumn, _mockBirthdateColumn, _mockGenderColumn);
    }

    [Theory]
    [InlineData("valid-student")]
    [InlineData("valid-student-gender-lower")]
    [InlineData("name-with-whitespace")]
    [InlineData("date-with-time")]
    public void CreateStudent_ShouldReturnStudent(string fileName)
    {
        // Arrange
        using var helper = new FileStreamHelper("ExcelParser", "Files", "Factories", $"{fileName}.xlsx");
        using IWorkbook workbook = WorkbookFactory.Create(helper.FileStream);
        IRow row = workbook.GetSheetAt(0).GetRow(0);

        // Act
        var result = _studentFactory.CreateStudent(row);

        // Assert
        result.Name.Should().Be("John Doe");
        result.Birthdate.Should().Be(new DateOnly(2012, 12, 21));
        result.Gender.Should().Be(Gender.Male);
    }

    [Theory]
    [InlineData("valid-female-student")]
    [InlineData("valid-female-student-gender-lower")]
    public void CreateStudent_ShouldReturnFemaleStudent(string fileName)
    {
        // Arrange
        using var helper = new FileStreamHelper("ExcelParser", "Files", "Factories", $"{fileName}.xlsx");
        using IWorkbook workbook = WorkbookFactory.Create(helper.FileStream);
        IRow row = workbook.GetSheetAt(0).GetRow(0);

        // Act
        var result = _studentFactory.CreateStudent(row);

        // Assert
        result.Name.Should().Be("Jane Doe");
        result.Birthdate.Should().Be(new DateOnly(2012, 12, 21));
        result.Gender.Should().Be(Gender.Female);
    }
}
