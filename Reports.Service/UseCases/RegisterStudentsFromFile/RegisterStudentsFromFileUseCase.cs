using FluentResults;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;
using Reports.Service.ExcelParser.Interfaces;

namespace Reports.Service.UseCases.RegisterStudentsFromFile;

/// <summary>
/// Use case for registering students from a file.
/// </summary>
public class RegisterStudentsFromFileUseCase
{
    private readonly IExcelParserService _excelFileHeaderService;
    private readonly IClassroomRepository _classroomRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterStudentsFromFileUseCase"/> class.
    /// </summary>
    /// <param name="excelParserService">The Excel parser service.</param>
    /// <param name="classroomRepository">The classroom repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public RegisterStudentsFromFileUseCase(
        IExcelParserService excelParserService,
        IClassroomRepository classroomRepository,
        IUnitOfWork unitOfWork)
    {
        _excelFileHeaderService = excelParserService;
        _classroomRepository = classroomRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Executes the use case asynchronously.
    /// </summary>
    /// <param name="request">The request data.</param>
    /// <returns>The result of the use case execution.</returns>
    public async Task<Result<RegisterStudentsFromFileResponse>> ExecuteAsync(RegisterStudentsFromFileRequest request)
    {
        var readStudentsResult = _excelFileHeaderService.ReadStudentsFromFile(request.Stream);

        if (readStudentsResult.IsFailed)
        {
            return new Result().WithErrors(readStudentsResult.Errors);
        }

        var classroom = new ClassRoom
        {
            Name = request.ClassroomName,
            Students = readStudentsResult.Value,
            TeacherId = request.TeacherId
        };

        _classroomRepository.Add(classroom);

        await _unitOfWork.SaveChangesAsync();

        return new RegisterStudentsFromFileResponse(classroom.Id);
    }
}
