using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Reports.Infrastructure.DbContexts;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    private const string ConnectionString = "ADMIN_CONNECTION_STRING";

    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionString);

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException($"Environment variable {ConnectionString} is not set.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new AppDbContext(optionsBuilder.Options);
    }
}
