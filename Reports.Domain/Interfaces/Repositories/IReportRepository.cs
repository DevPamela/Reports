using Reports.Domain.Models;

namespace Reports.Domain.Interfaces.Repositories;
public interface IReportRepository
{
    /// <summary>
    /// Add one student to the data store.
    /// </summary>
    /// <param name="student">The student to add.</param>
    /// <returns></returns>
    void Add(Report report);

    /// <summary>
    /// Retrieve all students from the data store.
    /// </summary>
    /// <returns>A List of students.</returns>
    Task<List<Report>> GetAllAsync(int id);

    /// <summary>
    /// Retrieve one student from the data store by its name.
    /// </summary>
    /// <param name="name">The student's name.</param>
    /// <returns>A student, or null in case it wasn't found.</returns>
    Task<Report?> GetByNameAsync(string name);

    /// <summary>
    /// Get Report by id.
    /// </summary>
    /// <param name="idDoRelatorio"></param>
    /// <returns></returns>
    Task<Report> GetReportById(int idDoRelatorio);
}