using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reports.Application.Settings;
using Reports.Domain.Interfaces.Repositories;
using Reports.Infrastructure.Repositories;

namespace Reports.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Adds the infrastructure services to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The application configuration properties.</param>
    /// <returns>A referente to the <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ReportsDatabaseSettings>(
            configuration.GetSection("ReportsDatabase"));

        services.AddSingleton<MongoDbContext>();

        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IGoalRepository, GoalRepository>();

        return services;
    }
}
