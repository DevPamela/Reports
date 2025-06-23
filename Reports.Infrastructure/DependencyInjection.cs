using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Interfaces.Service;
using Reports.Infrastructure.DbContexts;
using Reports.Infrastructure.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        try
        {
            var connectionString = configuration.GetConnectionString("ReportsConnection");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            options.UseOpenIddict();
            });

            services.AddScoped<IUnitOfWork>(sp =>
                sp.GetRequiredService<AppDbContext>());

            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IGoalRepository, GoalRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IAssessmentRepository, AssessmentRepository>();
            services.AddScoped<IClassroomRepository, ClassroomRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
            services.AddOpenIddict()
        .AddCore(options =>
        {
            // Configure OpenIddict to use the Entity Framework Core stores.
            options.UseEntityFrameworkCore()
                   .UseDbContext<AppDbContext>();
        })
        .AddServer(options =>
        {
            // Enable the token endpoint.
            options.SetTokenEndpointUris("/connect/token");

            // Enable the client credentials flow.
            options.AllowClientCredentialsFlow();

            // Register the signing and encryption credentials.
            options.AddDevelopmentEncryptionCertificate()
                   .AddDevelopmentSigningCertificate();

            // Register the ASP.NET Core host and configure the endpoints.
            options.UseAspNetCore()
                   .EnableTokenEndpointPassthrough();

            // Optionally set access token lifetime
            options.SetAccessTokenLifetime(TimeSpan.FromHours(1));
        })
        .AddValidation(options =>
        {
            // Enable JWT token validation.
            options.UseLocalServer();
            options.UseAspNetCore();
        });

            return services;
        }
        catch (global::System.Exception)
        {
            throw;
        }
    }
}