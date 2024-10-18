using Bogus;
using Bogus.DataSets;
using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using NSubstitute;
using Reports.Domain.Enums;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;
using Reports.Service.UseCases.Students.AddStudent;

namespace Reports.Service.Tests.UseCases.Students.AddStudent
{
    public class AddStudentUseCaseTests
    {
        private readonly IClassroomRepository _classroomRepository = Substitute.For<IClassroomRepository>();
        private readonly IStudentRepository _studentRepository = Substitute.For<IStudentRepository>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly Faker<AddStudentRequest> _requestFaker = new();
        private readonly AddStudentUseCase _useCase;

        public AddStudentUseCaseTests()
        {
            _requestFaker.CustomInstantiator(f =>
            {
                var gender = f.PickRandom(Enum.GetValues<Gender>());

                return new AddStudentRequest(
                    f.Name.FullName(gender == Gender.Female ? Name.Gender.Female : Name.Gender.Male),
                    DateOnly.FromDateTime(f.Person.DateOfBirth),
                    gender,
                    f.Random.Number(1, 100),
                    f.Random.Number(1, 100));
            });

            _useCase = new AddStudentUseCase(_classroomRepository, _studentRepository, _unitOfWork);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnClassroomNotFound_WhenClassroomDoesNotExist()
        {
            // Arrange
            var errorMessage = "Turma não encontrada.";
            var request = _requestFaker.Generate();
            _classroomRepository.ClassroomForTeacherExists(request.ClassroomId, request.TeacherId).Returns(false);

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            result.Should().BeFailure();
            result.Should().HaveError(errorMessage);
            _studentRepository.DidNotReceive().Add(Arg.Any<Student>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldAddStudentAndSaveChanges_WhenClassroomExists()
        {
            // Arrange
            var request = _requestFaker.Generate();
            _classroomRepository.ClassroomForTeacherExists(request.ClassroomId, request.TeacherId).Returns(true);

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            result.Should().BeSuccess();
            _studentRepository.Received(1).Add(Arg.Is<Student>(s =>
                s.Name == request.Name
                && s.Birthdate == request.Birthdate
                && s.Gender == request.Gender
                && s.ClassRoomId == request.ClassroomId
                && s.TeacherId == request.TeacherId));
            await _unitOfWork.Received(1).SaveChangesAsync();
        }
    }
}

