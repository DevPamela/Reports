using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;
using Reports.Infrastructure.DbContexts;
using Reports.Infrastructure.Repositories;

namespace Reports.Infrastructure.Tests;

public class StudentRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly AppDbContext _dbContext;
    private readonly StudentRepository _studentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StudentRepositoryTests(DatabaseFixture databaseFixture)
    {
        _dbContext = databaseFixture.DbContext;
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        _studentRepository = new StudentRepository(_dbContext);
        _unitOfWork = _dbContext;
    }

    [Fact]
    public async Task AddAsync_ShouldSaveStudentToDatabase()
    {
        // Arrange
        var student = StudentGenerator.GenerateOne();

        // Act
        _studentRepository.Add(student);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        _dbContext.ChangeTracker.Clear();
        var result = await _dbContext.Students
            .FirstAsync(s => s.Id == student.Id);

        result.Id.Should().NotBe(0);
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
        _dbContext.Students.AddRange(students);
        await _dbContext.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();

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
        _dbContext.Students.Add(student);
        await _dbContext.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();

        // Act
        var result = await _studentRepository.GetByNameAsync(student.Name);

        // Assert
        var studentFromDb = result.Should().BeOfType<Student>().Subject;
        studentFromDb.Name.Should().Be(student.Name);
        studentFromDb.Birthday.Should().Be(student.Birthday);
        studentFromDb.Absences.Should().Be(student.Absences);
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
}
