using Microsoft.EntityFrameworkCore;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Entities;
using Reports.Infrastructure.DbContexts;
using Polly;
using static NPOI.HSSF.Util.HSSFColor;

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
    public async Task<Goal?> GetByNameAsync(string goal, int teacherId) =>
        await _dbContext.Goals
            .FirstOrDefaultAsync(g => g.Name == goal & g.TeacherId == teacherId);

    /// <inheritdoc />
    public async Task<GirlsGoal?> GetGirlsGoalsByNameAndTeacherIdAsync(string name, int teacherId) =>
     await _dbContext.Goals
        .OfType<GirlsGoal>()
        .Where(g => g.Name == name && g.TeacherId == teacherId)
        .FirstOrDefaultAsync();

    /// <inheritdoc />
    public async Task<List<Goal>> GetGoalsByTeacherIdAsync(int teacherId)
    {
        return await _dbContext.Goals
            .Where(g => g.TeacherId == teacherId)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Goal?> GetByIdAsync(int id) =>
          await _dbContext.Goals
              .FirstOrDefaultAsync(s => s.Id == id);

    /// <inheritdoc />
    public async Task<List<Goal>> GetByIsSelectedProperty(int teacherId) =>
   await _dbContext.Goals
       .Where(g => g.IsActive == true & g.TeacherId == teacherId).ToListAsync();

    /// <inheritdoc />
    public async Task<GirlsGoal?> GetGirlsGoalByNameAsync(string goalName, int teacherId)
    {
        return await _dbContext.Goals
            .OfType<GirlsGoal>() 
            .Where(g => g.Name == goalName & g.TeacherId == teacherId)
            .FirstOrDefaultAsync();
    }
}
