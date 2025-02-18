using Reports.Domain.Entities;

namespace Reports.Domain.Interfaces.Service;

public interface IStudentService
{
    /// <summary>
    /// Add a new student.
    /// </summary>
    /// <param name="student"></param>
    /// <returns></returns>
    Task AddStudentAsync(Student student);

    /// <summary>
    /// Get the student list frm database.
    /// </summary>
    /// <returns></returns>
    Task<List<Student>> GetStudentsListAsync();

    /// <summary>
    /// Get student by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<Student?> GetStudentByNameAsync(string name);

    /// <summary>
    /// Get the student by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Student?> GetByIdAsync(int id);
    /// <summary>
    /// Get all studets selected for the report.
    /// </summary>
    /// <returns></returns>
    Task<List<Student?>> GetStudentByIsActiveAsync();

    /// <summary>
    /// Update student.
    /// </summary>
    /// <param name="student"></param>
    /// <returns></returns>
    Task UpdateStudent(Student student);

    /// <summary>
    /// delete a student.
    /// </summary>
    /// <param name="student"></param>
    /// <returns></returns>
    Task DeleteStudent(Student student);

    /// <summary>
    /// Marks the Student as selected for future saving in the database.
    /// </summary>
    /// <param name="students"></param>
    /// <returns>A boolean indicating that the student has been selected to write the report.</returns>
    Task SetStudentAsTrue(List<Student> students);
    /// <summary>
    /// Defines the number of absences for each student.
    /// </summary>
    /// <param name="student"></param>
    /// <returns></returns>
    Task SetAbsencesNumber(List<Student> student);
    /// <summary>
    /// Set the ‘IsSelected’ property to false, to prevent data from previous operations from being used in a new request.
    /// </summary>
    /// <param name="students"></param>
    /// <returns></returns>
    Task ClearSelection(List<Student> students);
    /// <summary>
    /// Searches for the property selected by the user.
    /// </summary>
    /// <returns></returns>
    Task<List<Student>> GetByIsSelectedProperty();
    /// <summary>
    /// Take a list of students from a specific classroom.
    /// </summary>
    /// <param name="selectedClasses">A list of Ids which will be used to search for the object with the corresponding id.</param>
    /// <returns></returns>
    Task<List<Student>> GetStudentsSelectedByClasses(IEnumerable<int> selectedClasses);
    /// <summary>
    /// Get a List of students.
    /// </summary>
    /// <param name="ids">a list of ids that will be used to search for the object with the corresponding id.</param>
    /// <returns></returns>
    Task<List<Student>> GetStudentsByIdListAsync(List<int> ids);
}
