using Reports.Domain.Enums;

namespace Reports.Service.UseCases.Students.AddStudent;

/// <summary>
/// Represents the request to add a student.
/// </summary>
public record AddStudentRequest(string Name, DateOnly Birthdate, Gender Gender, int ClassroomId, int TeacherId);
