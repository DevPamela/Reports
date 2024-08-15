using NPOI.SS.UserModel;
using Reports.Domain.Models;
using Reports.Service.ExcelParser.Interfaces;

namespace Reports.Service.ExcelParser.Factories;

/// <summary>
/// Represents a factory for creating instances of the Student class.
/// </summary>
public class StudentFactory : IStudentFactory
{
    private readonly IStudentNameColumn _nameColumnInfo;
    private readonly IStudentBirthdayColumn _birthdayColumnInfo;

    /// <summary>
    /// Initializes a new instance of the StudentFactory class.
    /// </summary>
    /// <param name="nameColumnInfo">The column information for the student name.</param>
    /// <param name="birthdayColumnInfo">The column information for the student birthday.</param>
    public StudentFactory(
        IStudentNameColumn nameColumnInfo,
        IStudentBirthdayColumn birthdayColumnInfo)
    {
        _nameColumnInfo = nameColumnInfo;
        _birthdayColumnInfo = birthdayColumnInfo;
    }

    /// <inheritdoc />
    public Student CreateStudent(IRow row)
    {
        var name = row.GetCell(_nameColumnInfo.Index).StringCellValue.Trim();
        var birthday = row.GetCell(_birthdayColumnInfo.Index).DateCellValue!.Value.Date;

        return new Student
        {
            Name = name,
            Birthday = birthday
        };
    }
}
