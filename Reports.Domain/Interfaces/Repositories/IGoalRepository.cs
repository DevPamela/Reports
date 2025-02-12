using Reports.Domain.Entities;

namespace Reports.Domain.Interfaces.Repositories;
public interface IGoalRepository
{
    /// <summary>
    /// Add one goal to the data store.
    /// </summary>
    /// <param name="goal">The goal to add.</param>
    /// <returns></returns>
    void Add(Goal goal);

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
    /// Get the goal by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Goal?> GetByIdAsync(int id);

    /// <summary>
    /// Set IsSelected property as true.
    /// </summary>
    /// <param name="goal"></param>
    /// <returns></returns>
    Task<List<Goal>> GetByIsSelectedProperty();

    /// <summary>
    /// Retrieve the goal from the data store by Teacher id.
    /// </summary>
    /// <param name="teacherId"></param>
    /// <returns></returns>
    Task<List<Goal>> GetGoalsByTeacherIdAsync(int teacherId);
    /// <summary>
    /// Retrieve one oal from the data store by its name and teacher id.
    /// </summary>
    /// <param name="name">The goal's name.</param>
    /// <returns>A goal, or null in case it wasn't found.</returns>
    Task<GirlsGoal?> GetGirlsGoalsByNameAndTeacherIdAsync(string name, int teacherId);

    /// <summary>
    ///Retrieve one GirlGoal from the data store by its name.
    /// </summary>
    /// <param name="goalName"></param>
    /// <returns></returns>
    Task<GirlsGoal?> GetGirlsGoalByNameAsync(string goalName);
    
    }