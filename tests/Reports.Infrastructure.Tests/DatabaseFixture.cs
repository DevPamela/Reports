using Microsoft.EntityFrameworkCore;
using Reports.Infrastructure.DbContexts;
using Testcontainers.MySql;

namespace Reports.Infrastructure.Tests;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MySqlContainer _mySqlContainer =
        new MySqlBuilder().Build();

    private DbContextOptions<AppDbContext> _dbContextOptions = null!;

    internal AppDbContext DbContext => new(_dbContextOptions);

    public async Task InitializeAsync()
    {
        await _mySqlContainer.StartAsync();

        var connectionString = _mySqlContainer.GetConnectionString();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        _dbContextOptions = optionsBuilder.Options;
    }

    public async Task DisposeAsync() =>
        await _mySqlContainer.DisposeAsync();
}