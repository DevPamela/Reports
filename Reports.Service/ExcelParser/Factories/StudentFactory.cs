using NPOI.SS.UserModel;
using Reports.Domain.Models;
using Reports.Service.ExcelParser.Interfaces;

namespace Reports.Service.ExcelParser.Factories;

/// <summary>
/// Represents a factory for creating instances of the Student class.
/// </summary>
public class StudentFactory : IStudentFactory
{
    private readonly IStudentNameColumn _nameColumn;
    private readonly IStudentBirthdayColumn _birthdayColumn;
    private readonly IStudentGenderColumn _genderColumn;

    /// <summary>
    /// Initializes a new instance of the StudentFactory class.
    /// </summary>
    /// <param name="nameColumn">The column information for the student name.</param>
    /// <param name="birthdayColumn">The column information for the student birthday.</param>
    /// <param name="genderColumn">The column information for the student gender.</param>
    public StudentFactory(
        IStudentNameColumn nameColumn,
        IStudentBirthdayColumn birthdayColumn,
        IStudentGenderColumn genderColumn)
    {
        _nameColumn = nameColumn;
        _birthdayColumn = birthdayColumn;
        _genderColumn = genderColumn;
    }

    /// <inheritdoc />
    public Student CreateStudent(IRow row)
    {
        var name = row.GetCell(_nameColumn.Index).StringCellValue.Trim();
        var birthday = row.GetCell(_birthdayColumn.Index).DateCellValue!.Value.Date;
        var gender = row.GetCell(_genderColumn.Index).StringCellValue.Trim();

        return new Student
        {
            Name = name,
            Birthday = birthday,
            Genre = gender,
        };
    }
}
