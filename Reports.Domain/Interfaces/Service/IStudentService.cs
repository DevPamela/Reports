using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using Reports.Domain.Models;

namespace Reports.Domain.Interfaces.Service
{
    public interface IStudentService
    {
        List<Student> ReadStudentsFromSheet(ISheet sheet, int columnIndexName, int columnIndexBirthday);
        Task<List<Student>> AddStudentByListAsync(IFormFile excelFile, string[] param);
        Task AddStudentAsync(Student student);
        Task<List<Student>> GetStudentsListAsync();
        Task<Student?> GetStudentByNameAsync(string name);
        Task<Student?> GetByIdAsync(int id);
        Task UpdateStudent(Student student);
        Task DeleteStudent(Student student);
        Task UpdateStudentList(List<Student> students);
        Task SetStudentAsTrue(List<Student> students);
        Task SetAbsencesNumber(List<Student> student);
        Task ClearSelection(List<Student> students);
    }
}
