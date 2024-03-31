using Reports.Domain.Models;

namespace Reports.Domain.Interfaces.Repositories;
public interface IStudentRepository
{
    Task AddOneAsync(Student student);
    Task<List<Student>> GetAllAsync();
    Task<Student?> GetByNameAsync(string name);
    Task UpdateOneStudentGoalsAsync(string studentName, IEnumerable<Goal> goals);
}