using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using Reports.Domain.Models;

namespace Reports.Domain.Interfaces
{
    public interface IStudentService
    {
        List<Student> ReadStudentsFromSheet(ISheet sheet, int columnIndexName, int columnIndexAbsence, int columnIndexBirthday);
        List<Student> AddStudentByList(IFormFile excelFile, string[] param);
        List<Student> GetStudentsList();
        Student GetStudentByName(string name);
        
    }
}
