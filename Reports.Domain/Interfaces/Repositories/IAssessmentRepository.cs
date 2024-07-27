using Reports.Domain.Models;

namespace Reports.Domain.Interfaces.Repositories;
public interface IAssessmentRepository
{
    /// <summary>
    /// Add one goal to the data store.
    /// </summary>
    /// <param name="goal">The goal to add.</param>
    /// <returns></returns>
    void Add(Assessment assesment);

    /// <summary>
    /// Retrieve all goals from the data store.
    /// </summary>
    /// <returns>A List of goals.</returns>
    Task<List<Assessment>> GetAllAsync();
    void Update(Assessment assessment);

    /// <summary>
    /// Retrieve one goal from the data store by its name.
    /// </summary>
    /// <param name="name">The goal's name.</param>
    /// <returns>A goal, or null in case it wasn't found.</returns>
    Task<Assessment?> GetByNameAsync(string name);
    Task<Assessment?> GetByIdAsync(int id);
    Task<Assessment?> GetByStudentIdAndGoalId(int studentId, int goalId);
    Task<Assessment?> GetByStudentIdAsync(int id);
}