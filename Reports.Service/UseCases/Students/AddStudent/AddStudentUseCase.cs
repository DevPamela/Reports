using FluentResults;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Entities;

namespace Reports.Service.UseCases.Students.AddStudent;

/// <summary>
/// Represents the use case for adding a student.
/// </summary>
public class AddStudentUseCase
{
    private readonly IClassroomRepository _classroomRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddStudentUseCase"/> class.
    /// </summary>
    /// <param name="classroomRepository">The classroom repository.</param>
    /// <param name="studentRepository">The student repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public AddStudentUseCase(
        IClassroomRepository classroomRepository,
        IStudentRepository studentRepository,
        IUnitOfWork unitOfWork)
    {
        _classroomRepository = classroomRepository;
        _studentRepository = studentRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Executes the use case asynchronously.
    /// </summary>
    /// <param name="request">The request data.</param>
    /// <returns>The result of the use case execution.</returns>
    public async Task<Result> ExecuteAsync(AddStudentRequest request)
    {
        var exists = await _classroomRepository.ClassroomForTeacherExists(request.ClassroomId, request.TeacherId);

        if (!exists)
        {
            return AddStudentErrors.ClassroomNotFound();
        }

        var student = new Student
        {
            Name = request.Name,
            Birthdate = request.Birthdate,
            Gender = request.Gender,
            ClassRoomId = request.ClassroomId,
            TeacherId = request.TeacherId
        };

        _studentRepository.Add(student);
        await _unitOfWork.SaveChangesAsync();

        return Result.Ok();
    }
}
