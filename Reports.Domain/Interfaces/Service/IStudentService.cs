using Reports.Domain.Models;

namespace Reports.Domain.Interfaces.Service;

public interface IStudentService
{
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
