using FluentAssertions;
using NPOI.SS.UserModel;
using NSubstitute;
using Reports.Service.ExcelParser.Factories;
using Reports.Service.ExcelParser.Interfaces;
using Reports.Tests.TestHelpers;

namespace Reports.Service.Tests.Factories;

public class StudentFactoryTests
{
    private readonly IStudentNameColumn _mockNameColumnInfo = Substitute.For<IStudentNameColumn>();
    private readonly IStudentBirthdayColumn _mockBirthdayColumnInfo = Substitute.For<IStudentBirthdayColumn>();
    private readonly StudentFactory _studentFactory;

    public StudentFactoryTests()
    {
        _mockNameColumnInfo.Index.Returns(0);
        _mockBirthdayColumnInfo.Index.Returns(1);
        _studentFactory = new StudentFactory(_mockNameColumnInfo, _mockBirthdayColumnInfo);
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
    }
}
