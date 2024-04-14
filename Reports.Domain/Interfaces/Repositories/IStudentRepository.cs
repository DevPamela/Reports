using Reports.Domain.Models;

namespace Reports.Domain.Interfaces.Repositories;
public interface IStudentRepository
{
    /// <summary>
    /// Add one student to the data store.
    /// </summary>
    /// <param name="student">The student to add.</param>
    /// <returns></returns>
    void Add(Student student);

    /// <summary>
    /// Retrieve all students from the data store.
    /// </summary>
    /// <returns>A List of students.</returns>
    Task<List<Student>> GetAllAsync();

    /// <summary>
    /// Retrieve one student from the data store by its name.
    /// </summary>
    /// <param name="name">The student's name.</param>
    /// <returns>A student, or null in case it wasn't found.</returns>
    Task<Student?> GetByNameAsync(string name);
}