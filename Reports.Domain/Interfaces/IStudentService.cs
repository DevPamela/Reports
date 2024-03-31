using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using Reports.Domain.Models;

namespace Reports.Domain.Interfaces
{
    public interface IStudentService
    {
        List<Student> ReadStudentsFromSheet(ISheet sheet, int columnIndexName, int columnIndexAbsence, int columnIndexBirthday);
        Task<List<Student>> AddStudentByListAsync(IFormFile excelFile, string[] param);
        Task<List<Student>> GetStudentsListAsync();
        Task<Student?> GetStudentByNameAsync(string name);
        
    }
}
