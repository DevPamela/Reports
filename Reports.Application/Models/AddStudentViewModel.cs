using Reports.Application.Enums;
using Reports.Domain.Models.Responses;

namespace Reports.Application.Models;

public record AddStudentViewModel(
    string? Name,
    DateOnly? Birthdate,
    Gender? Gender,
    int? ClassroomId,
    IEnumerable<ClassroomNameResponse> Classrooms);
