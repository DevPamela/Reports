using FluentAssertions;
using NPOI.SS.UserModel;
using NSubstitute;
using Reports.Service.ExcelFile.Factories;
using Reports.Service.ExcelFile.Interfaces;
using Reports.Service.Tests.ExcelFile.Helpers;

namespace Reports.Service.Tests.ExcelFile.Factories;

public class StudentFactoryTests
{
    private readonly StudentFactory _studentFactory;
    private readonly INameColumnInfo _mockNameColumnInfo;
    private readonly IBirthdayColumnInfo _mockBirthdayColumnInfo;

    public StudentFactoryTests()
    {
        _mockNameColumnInfo = Substitute.For<INameColumnInfo>();
        _mockNameColumnInfo.Index.Returns(0);

        _mockBirthdayColumnInfo = Substitute.For<IBirthdayColumnInfo>();
        _mockBirthdayColumnInfo.Index.Returns(1);

        _studentFactory = new StudentFactory(_mockNameColumnInfo, _mockBirthdayColumnInfo);
    }

    [Theory]
    [InlineData("valid-student")]
    [InlineData("name-with-whitespace")]
    [InlineData("date-with-time")]
    public void CreateStudent_ShouldReturnStudent(string fileName)
    {
        // Arrange
        using var stream = new FileStreamHelper("Factories", fileName).FileStream;
        using IWorkbook workbook = WorkbookFactory.Create(stream);
        IRow row = workbook.GetSheetAt(0).GetRow(0);

        // Act
        var result = _studentFactory.CreateStudent(row);

        // Assert
        result.Name.Should().Be("John Doe");
        result.Birthday.Should().Be(new DateTime(2012, 12, 21, 0, 0, 0, DateTimeKind.Local));
    }
}
