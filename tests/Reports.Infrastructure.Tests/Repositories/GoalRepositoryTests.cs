using FluentAssertions;
using MongoDB.Driver;
using Reports.Infrastructure.Repositories;

namespace Reports.Infrastructure.Tests;

public class GoalRepositoryTests : IClassFixture<DatabaseFixture>, IAsyncLifetime
{
    private readonly MongoDbContext _dbContext;
    private readonly GoalRepository _goalRepository;

    public GoalRepositoryTests(DatabaseFixture databaseFixture)
    {
        _dbContext = databaseFixture.DbContext;
        _goalRepository = new GoalRepository(_dbContext);
    }

    [Fact]
    public async Task AddOneAsync_ShouldSaveGoalToDatabase()
    {
        // Arrange
        var goal = GoalGenerator.GenerateOne();

        // Act
        await _goalRepository.AddOneAsync(goal);

        // Assert
        var result = await _dbContext.GoalsCollection
            .Find(g => g.Name == goal.Name)
            .FirstOrDefaultAsync();

        result.Id.Should().NotBeEmpty();
        result.Name.Should().Be(goal.Name);
        result.IsSatisfactory.Should().Be(goal.IsSatisfactory);
        result.AtendedDescription.Should().Be(goal.AtendedDescription);
        result.ParciallyAtendedDescription.Should().Be(goal.ParciallyAtendedDescription);
        result.UnatendedDescription.Should().Be(goal.UnatendedDescription);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfGoals()
    {
        // Arrange
        var goals = GoalGenerator.GenerateMany();
        await _dbContext.GoalsCollection.InsertManyAsync(goals);

        // Act
        var result = await _goalRepository.GetAllAsync();

        // Assert
        result.Should().HaveCount(goals.Count);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnGoal_WhenGoalExists()
    {
        // Arrange
        var goal = GoalGenerator.GenerateOne();
        await _dbContext.GoalsCollection.InsertOneAsync(goal);

        // Act
        var result = await _goalRepository.GetByNameAsync(goal.Name);

        // Assert
        result.Name.Should().Be(goal.Name);
        result.AtendedDescription.Should().Be(goal.AtendedDescription);
        result.ParciallyAtendedDescription.Should().Be(goal.ParciallyAtendedDescription);
        result.UnatendedDescription.Should().Be(goal.UnatendedDescription);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnNull_WhenGoalDoesNotExist()
    {
        // Arrange
        var anyGoal = "anything";

        // Act
        var result = await _goalRepository.GetByNameAsync(anyGoal);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateOneGoalDescriptionAsync_ShouldUpdateGoalDescription()
    {
        // Arrange
        var goal = GoalGenerator.GenerateOne();
        var description = "Updated Description";
        await _dbContext.GoalsCollection.InsertOneAsync(goal);

        // Act
        await _goalRepository.UpdateOneGoalDescriptionAsync(
            goal.Name, nameof(goal.AtendedDescription), description);

        // Assert
        var result = await _dbContext.GoalsCollection
            .Find(g => g.Name == goal.Name)
            .FirstOrDefaultAsync();

        result.AtendedDescription.Should().Be(description);
    }

    public async Task InitializeAsync() =>
        await _dbContext.GoalsCollection
            .DeleteManyAsync(_ => true);

    public Task DisposeAsync() =>
        Task.CompletedTask;
}
