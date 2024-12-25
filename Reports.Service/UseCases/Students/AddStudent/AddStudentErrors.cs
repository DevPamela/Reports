using FluentResults;

namespace Reports.Service.UseCases.Students.AddStudent;

/// <summary>
/// Represents the errors related to adding a Student.
/// </summary>
public class AddStudentErrors : Error
{
    private AddStudentErrors(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="AddStudentErrors"/> for the case when the Classroom does not exist for the Teacher.
    /// </summary>
    /// <returns>A new instance of the <see cref="AddStudentErrors"/> class.</returns>
    public static AddStudentErrors ClassroomNotFound() =>
        new("Turma não encontrada.");
}