using FluentAssertions;
using MongoDB.Driver;
using Reports.Infrastructure.Repositories;

namespace Reports.Infrastructure.Tests;

public class StudentRepositoryTests : IClassFixture<DatabaseFixture>, IAsyncLifetime
{
    private readonly MongoDbContext _dbContext;
    private readonly StudentRepository _studentRepository;

    public StudentRepositoryTests(DatabaseFixture databaseFixture)
    {
        _dbContext = databaseFixture.DbContext;
        _studentRepository = new StudentRepository(_dbContext);
    }

    [Fact]
    public async Task AddOneAsync_ShouldSaveStudentToDatabase()
    {
        // Arrange
        var student = StudentGenerator.GenerateOne();

        // Act
        await _studentRepository.AddOneAsync(student);

        // Assert
        var result = await _dbContext.StudentsCollection
            .Find(s => s.Name == student.Name)
            .FirstOrDefaultAsync();

        result.Id.Should().NotBeEmpty();
        result.Name.Should().Be(student.Name);
        result.Genre.Should().Be(student.Genre);
        result.Birthday.Should().Be(student.Birthday);
        result.Absences.Should().Be(student.Absences);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfStudents()
    {
        // Arrange
        var students = StudentGenerator.GenerateMany();
        await _dbContext.StudentsCollection.InsertManyAsync(students);

        // Act
        var result = await _studentRepository.GetAllAsync();

        // Assert
        result.Should().HaveCount(students.Count);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnStudent_WhenStudentExists()
    {
        // Arrange
        var student = StudentGenerator.GenerateOne();
        await _dbContext.StudentsCollection.InsertOneAsync(student);

        // Act
        var result = await _studentRepository.GetByNameAsync(student.Name);

        // Assert
        result.Name.Should().Be(student.Name);
        result.Birthday.Should().Be(student.Birthday);
        result.Absences.Should().Be(student.Absences);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnNull_WhenStudentDoesNotExist()
    {
        // Arrange
        var anyName = "Bob";

        // Act
        var result = await _studentRepository.GetByNameAsync(anyName);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateOneStudentGoalsAsync_ShouldUpdateStudentGoals()
    {
        // Arrange
        var student = StudentGenerator.GenerateOne();
        var goals = GoalGenerator.GenerateMany();
        await _dbContext.StudentsCollection.InsertOneAsync(student);

        // Act
        await _studentRepository.UpdateOneStudentGoalsAsync(student.Name, goals);

        // Assert
        var result = await _dbContext.StudentsCollection
            .Find(s => s.Name == student.Name)
            .FirstOrDefaultAsync();

        result.Goals.Should().HaveCount(goals.Count);
    }

    public async Task InitializeAsync() =>
        await _dbContext.StudentsCollection
            .DeleteManyAsync(_ => true);

    public Task DisposeAsync() =>
        Task.CompletedTask;
}
