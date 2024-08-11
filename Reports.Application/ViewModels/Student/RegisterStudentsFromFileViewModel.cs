namespace Reports.Application.ViewModels.Student;

/// <summary>
/// Represents a view model for registering students from a file.
/// </summary>
public record RegisterStudentsFromFileViewModel(string ClassroomName, IFormFile ExcelFile);
