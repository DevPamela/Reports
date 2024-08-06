using NPOI.SS.UserModel;
using Reports.Domain.Models;
using Reports.Service.ExcelFile.Interfaces;

namespace Reports.Service.ExcelFile.Factories;
public class StudentFactory : IStudentFactory
{
    private readonly INameColumnInfo _nameColumnInfo;
    private readonly IBirthdayColumnInfo _birthdayColumnInfo;

    public StudentFactory(
        INameColumnInfo nameColumnInfo,
        IBirthdayColumnInfo birthdayColumnInfo)
    {
        _nameColumnInfo = nameColumnInfo;
        _birthdayColumnInfo = birthdayColumnInfo;
    }

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
