namespace Reports.Service.UseCases.Students.RegisterStudentsFromFile;

/// <summary>
/// Represents the request to register students from a file.
/// </summary>
public record RegisterStudentsFromFileRequest(Stream Stream, string ClassroomName, int TeacherId);
