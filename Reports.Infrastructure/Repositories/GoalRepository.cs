using Microsoft.EntityFrameworkCore;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;
using Reports.Infrastructure.DbContexts;

namespace Reports.Infrastructure.Repositories;
internal class GoalRepository : IGoalRepository
{
    private readonly AppDbContext _dbContext;

    public GoalRepository(AppDbContext dbContext) =>
        _dbContext = dbContext;

    /// <inheritdoc />
    public void Add(Goal goal) =>
        _dbContext.Goals.Add(goal);

    /// <inheritdoc />
    public async Task<List<Goal>> GetAllAsync() =>
        await _dbContext.Goals.ToListAsync();

    /// <inheritdoc />
    public async Task<Goal?> GetByNameAsync(string name) =>
        await _dbContext.Goals
            .FirstOrDefaultAsync(g => g.Name == name);
    public async Task<Goal?> GetByIdAsync(int id) =>
          await _dbContext.Goals
              .FirstOrDefaultAsync(s => s.Id == id);

}
