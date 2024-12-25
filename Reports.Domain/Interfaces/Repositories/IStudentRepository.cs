using Reports.Domain.Entities;

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
    /// Delete a student from the database.
    /// </summary>
    /// <param name="student"></param>
    void DeleteAsync(Student student);
 
    /// <summary>
    /// Retrieve one student from the data store by its name.
    /// </summary>
    /// <param name="name">The student's name.</param>
    /// <returns>A student, or null in case it wasn't found.</returns>
     Task<Student?> GetByNameAsync(string name);

    /// <summary>
    /// Get by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Student?> GetByIdAsync(int id);

    /// <summary>
    /// Update the student.
    /// </summary>
    /// <param name="student"></param>
    void Update(Student student);
    Task<List<Student>> GetStudentsSelectedByClasses(IEnumerable<int> selectedClasses);

    /// <summary>
    /// Get the student by classroom id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Student?> GetByClassroomIdAsync(int id);

    /// <summary>
    /// Get pensar nisso
    /// </summary>
    /// <returns></returns>
    Task<List<Student>> GetByIsSelectedProperty();

    /// <summary>
    /// Get the student list by a list o ids.
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<List<Student>> GetStudentsByIdListAsync(List<int> ids);
}