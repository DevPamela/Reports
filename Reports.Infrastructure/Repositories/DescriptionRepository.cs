using Microsoft.EntityFrameworkCore;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;
using Reports.Infrastructure.DbContexts;

namespace Reports.Infrastructure.Repositories;
internal class DescriptionRepository : IDescriptionRepository
{
    private readonly AppDbContext _dbContext;

    public DescriptionRepository(AppDbContext dbContext) =>
        _dbContext = dbContext;

    /// <inheritdoc />
    public void Add(Description description) =>
        _dbContext.Descriptions.Add(description);

    /// <inheritdoc />
    public async Task<List<Description>> GetAllAsync() =>
        await _dbContext.Descriptions.ToListAsync();

    public void Update(Description description) =>
        _dbContext.Descriptions.Update(description);

    /// <inheritdoc />
    public async Task<Description?> GetByGoalId(int id) =>
        await _dbContext.Descriptions
            .FirstOrDefaultAsync(a => a.GoalId == id);
}
