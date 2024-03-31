using Reports.Domain.Models;

namespace Reports.Domain.Interfaces.Repositories;
public interface IGoalRepository
{
    Task AddOneAsync(Goal goal);
    Task<List<Goal>> GetAllAsync();
    Task<Goal?> GetByNameAsync(string name);
    Task UpdateOneGoalDescriptionAsync(string name, string field, string description);
}