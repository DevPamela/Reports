using Reports.Domain.Models;

namespace Reports.Domain.Interfaces.Repositories;
public interface IGoalRepository
{
    /// <summary>
    /// Persist one goal in the data store.
    /// </summary>
    /// <param name="goal">The goal to persist.</param>
    /// <returns></returns>
    Task AddOneAsync(Goal goal);

    /// <summary>
    /// Retrieve all goals from the data store.
    /// </summary>
    /// <returns>A List of goals.</returns>
    Task<List<Goal>> GetAllAsync();

    /// <summary>
    /// Retrieve one goal from the data store by its name.
    /// </summary>
    /// <param name="name">The goal's name.</param>
    /// <returns>A goal, or null in case it wasn't found.</returns>
    Task<Goal?> GetByNameAsync(string name);

    /// <summary>
    /// Update a goal's decription field.
    /// </summary>
    /// <param name="name">The name of the goal to update.</param>
    /// <param name="field">The decription field to update.</param>
    /// <param name="description">The new description.</param>
    /// <returns></returns>
    Task UpdateOneGoalDescriptionAsync(string name, string field, string description);
}