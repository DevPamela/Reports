using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reports.Application.Settings;
using Reports.Domain.Interfaces.Repositories;
using Reports.Infrastructure.Repositories;

namespace Reports.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastrucuture(
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
