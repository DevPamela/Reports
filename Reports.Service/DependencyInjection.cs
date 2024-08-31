using Microsoft.Extensions.DependencyInjection;
using Reports.Service.ExcelParser;
using Reports.Service.ExcelParser.Factories;
using Reports.Service.ExcelParser.Interfaces;
using Reports.Service.ExcelParser.Validators.StudentBirthdayColumn;
using Reports.Service.ExcelParser.Validators.StudentGenderColumn;
using Reports.Service.ExcelParser.Validators.StudentNameColumn;
using Reports.Service.UseCases.RegisterStudentsFromFile;

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

        services.AddScoped<RegisterStudentsFromFileUseCase>();
        services.AddScoped<IStudentFactory, StudentFactory>();

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

        services.AddScoped<StudentBirthdayColumnValidator>();
        services.AddScoped<IColumnValidator>(sp =>
            sp.GetRequiredService<StudentBirthdayColumnValidator>());
        services.AddScoped<IStudentBirthdayColumn>(sp =>
            sp.GetRequiredService<StudentBirthdayColumnValidator>());

        services.AddScoped<StudentGenderColumnValidator>();
        services.AddScoped<IColumnValidator>(sp =>
            sp.GetRequiredService<StudentGenderColumnValidator>());
        services.AddScoped<IStudentGenderColumn>(sp =>
            sp.GetRequiredService<StudentGenderColumnValidator>());
    }
}
