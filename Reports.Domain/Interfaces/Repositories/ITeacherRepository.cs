using Reports.Domain.Models;

namespace Reports.Domain.Interfaces.Repositories;
public interface ITeacherRepository
{
    /// <summary>
    /// Add one goal to the data store.
    /// </summary>
    /// <param name="goal">The goal to add.</param>
    /// <returns></returns>
    void Add(Teacher teacher);

    void Update(Teacher teacher);

    /// <summary>
    /// Retrieve one goal from the data store by its name.
    /// </summary>
    /// <param name="name">The goal's name.</param>
    /// <returns>A goal, or null in case it wasn't found.</returns>
    Task<Teacher?> GetByIdAsync(int id);
}