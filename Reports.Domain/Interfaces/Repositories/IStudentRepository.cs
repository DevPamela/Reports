using Reports.Domain.Models;

namespace Reports.Domain.Interfaces.Repositories;
public interface IStudentRepository
{
    /// <summary>
    /// Persist one student in the data store.
    /// </summary>
    /// <param name="student">The student to persist.</param>
    /// <returns></returns>
    Task AddOneAsync(Student student);

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

    /// <summary>
    /// Update a student with a collection of goals.
    /// </summary>
    /// <param name="studentName">The name of the student to update.</param>
    /// <param name="goals">The new collection of goals.</param>
    /// <returns></returns>
    Task UpdateOneStudentGoalsAsync(string studentName, IEnumerable<Goal> goals);
}