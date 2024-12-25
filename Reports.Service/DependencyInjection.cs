using Microsoft.Extensions.DependencyInjection;
using Reports.Domain.Interfaces.Service;
using Reports.Service.Assessments;
using Reports.Service.Classroom;
using Reports.Service.ExcelParser;
using Reports.Service.ExcelParser.Factories;
using Reports.Service.ExcelParser.Interfaces;
using Reports.Service.ExcelParser.Validators.StudentBirthdateColumn;
using Reports.Service.ExcelParser.Validators.StudentGenderColumn;
using Reports.Service.ExcelParser.Validators.StudentNameColumn;
using Reports.Service.Students;
using Reports.Service.UseCases.RegisterStudentsFromFile;
using Reports.Service.UseCases.Students.AddStudent;

namespace Reports.Service;

/// <summary>
/// Provides dependency injection configuration for the service layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the service layer dependencies to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddServiceLayer(this IServiceCollection services)
    {
        AddExcelParser(services);
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IFormatterService, FormatterService>();
        services.AddScoped<IGoalService, GoalService>();
        services.AddScoped<IAssessmentService, AssessmentService>();
        services.AddScoped<IClassroomService, ClassroomService>();
        services.AddScoped<ITeacherService, TeacherService>();
        services.AddScoped<RegisterStudentsFromFileUseCase>();
        services.AddScoped<IStudentFactory, StudentFactory>();
        services.AddScoped<AddStudentUseCase>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    private static void AddExcelParser(IServiceCollection services)
    {
        services.AddScoped<IExcelParserService, ExcelParserService>();

        services.AddScoped<StudentNameColumnValidator>();
        services.AddScoped<IColumnValidator>(sp =>
            sp.GetRequiredService<StudentNameColumnValidator>());
        services.AddScoped<IStudentNameColumn>(sp =>
            sp.GetRequiredService<StudentNameColumnValidator>());

        services.AddScoped<StudentBirthdateColumnValidator>();
        services.AddScoped<IColumnValidator>(sp =>
            sp.GetRequiredService<StudentBirthdateColumnValidator>());
        services.AddScoped<IStudentBirthdateColumn>(sp =>
            sp.GetRequiredService<StudentBirthdateColumnValidator>());

        services.AddScoped<StudentGenderColumnValidator>();
        services.AddScoped<IColumnValidator>(sp =>
            sp.GetRequiredService<StudentGenderColumnValidator>());
        services.AddScoped<IStudentGenderColumn>(sp =>
            sp.GetRequiredService<StudentGenderColumnValidator>());
    }
}
