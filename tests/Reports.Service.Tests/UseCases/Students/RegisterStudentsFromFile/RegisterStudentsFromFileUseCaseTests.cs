using Bogus;
using Bogus.DataSets;
using FluentAssertions;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;
using NSubstitute;
using Reports.Domain.Enums;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;
using Reports.Service.ExcelParser.Interfaces;
using Reports.Service.UseCases.Students.RegisterStudentsFromFile;

namespace Reports.Service.Tests.UseCases.Students.RegisterStudentsFromFile;

public class RegisterStudentsFromFileUseCaseTests
{
    private readonly IExcelParserService _mockExcelFileService = Substitute.For<IExcelParserService>();
    private readonly IClassroomRepository _mockClassroomRepository = Substitute.For<IClassroomRepository>();
    private readonly IUnitOfWork _mockUnitOfWork = Substitute.For<IUnitOfWork>();
    private readonly Faker<Student> _studentFaker = new();
    private readonly Faker<RegisterStudentsFromFileRequest> _requestFaker = new();
    private readonly RegisterStudentsFromFileUseCase _useCase;

    public RegisterStudentsFromFileUseCaseTests()
    {
        _studentFaker
            .RuleFor(s => s.Birthdate, f => DateOnly.FromDateTime(f.Person.DateOfBirth))
            .RuleFor(s => s.Gender, f => f.PickRandom(Enum.GetValues<Gender>()))
            .RuleFor(s => s.Name, (f, s) =>
                f.Name.FullName(s.Gender == Gender.Female ? Name.Gender.Female : Name.Gender.Male));

        _requestFaker.CustomInstantiator(f =>
            new RegisterStudentsFromFileRequest(
                new MemoryStream(),
                f.Lorem.Word(),
                f.Random.Int(1, 100)));

        _useCase = new RegisterStudentsFromFileUseCase(
            _mockExcelFileService,
            _mockClassroomRepository,
            _mockUnitOfWork);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnErrors_WhenReadStudentsFromFileFails()
    {
        // Arrange
        var errorMessage = "Error reading students";
        var readStudentsResult = Result.Fail<List<Student>>(errorMessage);
        _mockExcelFileService.ReadStudentsFromFile(Arg.Any<Stream>()).Returns(readStudentsResult);

        var request = _requestFaker.Generate();

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.Should().BeFailure();
        result.Should().HaveError(errorMessage);
        _mockClassroomRepository.DidNotReceive().Add(Arg.Any<Domain.Models.ClassRoom>());
        await _mockUnitOfWork.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task ExecuteAsync_ShouldAddClassroomAndSaveChanges_WhenReadStudentsFromFileSucceeds()
    {
        // Arrange
        var students = new List<Student> { _studentFaker.Generate() };
        var readStudentsResult = Result.Ok(students);
        _mockExcelFileService.ReadStudentsFromFile(Arg.Any<Stream>()).Returns(readStudentsResult);

        var request = _requestFaker.Generate();

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.Should().BeSuccess();
        _mockClassroomRepository.Received(1).Add(Arg.Is<Domain.Models.ClassRoom>(c =>
            c.Name == request.ClassroomName
            && c.TeacherId == request.TeacherId
            && c.Students == students));
        await _mockUnitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task ExecuteAsync_ShouldAddTeacherIdToStudents_WhenReadStudentsFromFileSucceeds()
    {
        // Arrange
        var students = new List<Student> { _studentFaker.Generate() };
        var readStudentsResult = Result.Ok(students);
        _mockExcelFileService.ReadStudentsFromFile(Arg.Any<Stream>()).Returns(readStudentsResult);

        var request = _requestFaker.Generate();

        // Act
        var result = await _useCase.ExecuteAsync(request);

        // Assert
        result.Should().BeSuccess();
        students.Should().AllSatisfy(s => s.TeacherId.Should().Be(request.TeacherId));
    }
}