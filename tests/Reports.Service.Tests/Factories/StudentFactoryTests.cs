using FluentAssertions;
using NPOI.SS.UserModel;
using NSubstitute;
using Reports.Service.ExcelParser.Factories;
using Reports.Service.ExcelParser.Interfaces;
using Reports.Tests.TestHelpers.Files;

namespace Reports.Service.Tests.Factories;

public class StudentFactoryTests
{
    private readonly IStudentNameColumn _mockNameColumn = Substitute.For<IStudentNameColumn>();
    private readonly IStudentBirthdayColumn _mockBirthdayColumn = Substitute.For<IStudentBirthdayColumn>();
    private readonly IStudentGenderColumn _mockGenderColumn = Substitute.For<IStudentGenderColumn>();
    private readonly StudentFactory _studentFactory;

    public StudentFactoryTests()
    {
        _mockNameColumn.Index.Returns(0);
        _mockBirthdayColumn.Index.Returns(1);
        _mockGenderColumn.Index.Returns(2);
        _studentFactory = new StudentFactory(_mockNameColumn, _mockBirthdayColumn, _mockGenderColumn);
    }

    [Theory]
    [InlineData("valid-student.xlsx")]
    [InlineData("name-with-whitespace.xlsx")]
    [InlineData("date-with-time.xlsx")]
    public void CreateStudent_ShouldReturnStudent(string fileName)
    {
        // Arrange
        using var helper = new FileStreamHelper("ExcelParser", "Files", "Factories", fileName);
        using IWorkbook workbook = WorkbookFactory.Create(helper.FileStream);
        IRow row = workbook.GetSheetAt(0).GetRow(0);

        // Act
        var result = _studentFactory.CreateStudent(row);

        // Assert
        result.Name.Should().Be("John Doe");
        result.Birthday.Should().Be(new DateTime(2012, 12, 21, 0, 0, 0, DateTimeKind.Local));
        result.Genre.Should().Be("Masculino");
    }
}
