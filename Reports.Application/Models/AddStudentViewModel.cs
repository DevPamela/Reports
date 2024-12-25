using Reports.Domain.Entities.Responses;
using Reports.Domain.Enums;

namespace Reports.Application.Models;

public record AddStudentViewModel(
    string? Name,
    DateOnly? Birthdate,
    Gender? Gender,
    int? ClassroomId,
    IEnumerable<ClassroomNameResponse> Classrooms);
