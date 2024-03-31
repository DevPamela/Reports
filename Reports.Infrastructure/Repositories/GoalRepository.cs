using MongoDB.Driver;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;

namespace Reports.Infrastructure.Repositories;
internal class GoalRepository : IGoalRepository
{
    private readonly MongoDbContext _dbContext;

    public GoalRepository(MongoDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task AddOneAsync(Goal goal) =>
        await _dbContext.GoalsCollection
            .InsertOneAsync(goal);

    public async Task<List<Goal>> GetAllAsync() =>
        await _dbContext.GoalsCollection
            .Find(_ => true)
            .ToListAsync();

    public async Task<Goal?> GetByNameAsync(string name) =>
        await _dbContext.GoalsCollection
            .Find(g => g.Name == name)
            .FirstOrDefaultAsync();

    public async Task UpdateOneGoalDescriptionAsync(string name, string field, string description)
    {
        var update = Builders<Goal>.Update.Set(field, description);
        await _dbContext.GoalsCollection.UpdateOneAsync(g => g.Name == name, update);
    }
}
