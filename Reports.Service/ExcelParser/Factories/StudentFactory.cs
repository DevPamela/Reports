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
    private readonly IStudentBirthdateColumn _birthdateColumn;
    private readonly IStudentGenderColumn _genderColumn;

    /// <summary>
    /// Initializes a new instance of the StudentFactory class.
    /// </summary>
    /// <param name="nameColumn">The column information for the student name.</param>
    /// <param name="birthdateColumn">The column information for the student birthdate.</param>
    /// <param name="genderColumn">The column information for the student gender.</param>
    public StudentFactory(
        IStudentNameColumn nameColumn,
        IStudentBirthdateColumn birthdateColumn,
        IStudentGenderColumn genderColumn)
    {
        _nameColumn = nameColumn;
        _birthdateColumn = birthdateColumn;
        _genderColumn = genderColumn;
    }

    /// <inheritdoc />
    public Student CreateStudent(IRow row)
    {
        var name = row.GetCell(_nameColumn.Index).StringCellValue.Trim();
        var birthdate = row.GetCell(_birthdateColumn.Index).DateCellValue!.Value.Date;
        var gender = row.GetCell(_genderColumn.Index).StringCellValue.Trim();

        return new Student
        {
            Name = name,
            Birthday = birthdate,
            Genre = gender,
        };
    }
}
