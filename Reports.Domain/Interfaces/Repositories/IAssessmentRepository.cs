using Reports.Domain.Entities;

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

    /// <summary>
    /// Update a existent <see cref="Assessment"/>.
    /// </summary>
    /// <param name="assessment"></param>
    void Update(Assessment assessment);

    /// <summary>
    /// Retrieve one <see cref="Assessment"> from the data store by its name.
    /// </summary>
    /// <param name="name">The goal's name.</param>
    /// <returns>A goal, or null in case it wasn't found.</returns>
    Task<Assessment?> GetByNameAsync(string name);

    /// <summary>
    ///  Retrieve one <see cref="Assessment"> from the data store by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Assessment?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieve a <see cref="Assessment"> from the data store by studentId and goalId.
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="goalId"></param>
    /// <returns></returns>
    Task<Assessment?> GetByStudentIdAndGoalId(int studentId, int goalId);

    /// <summary>
    /// Retrieve a <see cref="Assessment"> from the data store by studentId.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<List<Assessment?>> GetByStudentIdAsync(int id);

    /// <summary>
    /// Delete all <see cref="Assessment"/>from the data base.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAllAsync(int id);
}