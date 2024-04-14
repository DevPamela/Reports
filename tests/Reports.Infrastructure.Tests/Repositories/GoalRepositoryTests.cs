using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;
using Reports.Infrastructure.DbContexts;
using Reports.Infrastructure.Repositories;

namespace Reports.Infrastructure.Tests;

public class GoalRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly AppDbContext _dbContext;
    private readonly GoalRepository _goalRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GoalRepositoryTests(DatabaseFixture databaseFixture)
    {
        _dbContext = databaseFixture.DbContext;
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        _goalRepository = new GoalRepository(_dbContext);
        _unitOfWork = _dbContext;
    }

    [Fact]
    public async Task AddAsync_ShouldSaveGoalToDatabase()
    {
        // Arrange
        var goal = GoalGenerator.GenerateOne();

        // Act
        _goalRepository.Add(goal);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        _dbContext.ChangeTracker.Clear();
        var result = await _dbContext.Goals
            .FirstAsync(g => g.Id == goal.Id);

        result.Id.Should().NotBe(0);
        result.Name.Should().Be(goal.Name);
        result.AtendedDescription.Should().Be(goal.AtendedDescription);
        result.ParciallyAtendedDescription.Should().Be(goal.ParciallyAtendedDescription);
        result.UnatendedDescription.Should().Be(goal.UnatendedDescription);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfGoals()
    {
        // Arrange
        var goals = GoalGenerator.GenerateMany();
        _dbContext.Goals.AddRange(goals);
        await _dbContext.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();

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
        _dbContext.Goals.Add(goal);
        await _dbContext.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();

        // Act
        var result = await _goalRepository.GetByNameAsync(goal.Name);

        // Assert
        var goalFromDb = result.Should().BeOfType<Goal>().Subject;
        goalFromDb.Name.Should().Be(goal.Name);
        goalFromDb.AtendedDescription.Should().Be(goal.AtendedDescription);
        goalFromDb.ParciallyAtendedDescription.Should().Be(goal.ParciallyAtendedDescription);
        goalFromDb.UnatendedDescription.Should().Be(goal.UnatendedDescription);
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
}
