using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Reports.Domain.Entities;
using Reports.Infrastructure.DbContexts;
using Respawn;
using System.Data.Common;
using Testcontainers.MySql;
using Xunit;

namespace Reports.Tests.TestHelpers.IntegrationTesting;

/// <summary>
/// Represents a shared fixture for integration tests.
/// </summary>
public class SharedFixture : IAsyncLifetime
{
    private const string DbName = "reportsdbtest";

    private readonly bool _useContainers = Convert.ToBoolean(Environment.GetEnvironmentVariable("USE_CONTAINERS"));

    private MySqlContainer? _mySqlContainer;
    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    /// <summary>
    /// Gets the connection string for the MySQL database.
    /// </summary>
    public string ConnectionString { get; private set; } = default!;

    /// <summary>
    /// Gets the instance of the application database context.
    /// </summary>
    public AppDbContext SharedDbContext { get; private set; } = default!;

    /// <summary>
    /// Gets the teacher instance.
    /// </summary>
    public Teacher Teacher { get; } = new()
    {
        Id = 1,
        Name = "Teacher 1"
    };

    /// <summary>
    /// Seeds the database with initial data.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SeedAsync()
    {
        SharedDbContext.Teacher.Add(Teacher);
        await SharedDbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Resets the database to its initial state.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        if (_useContainers)
        {
            await SetupContainers();
        }

        GetConnectionString();

        await CreateDatabase();
        await SetupRespawner();
    }

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        await SharedDbContext.DisposeAsync();
        await _dbConnection.DisposeAsync();

        if (_useContainers)
        {
            await _mySqlContainer!.DisposeAsync();
        }
    }

    private async Task SetupContainers()
    {
        _mySqlContainer = new MySqlBuilder()
            .WithDatabase(DbName)
            .Build();

        await _mySqlContainer.StartAsync();
    }

    private void GetConnectionString()
    {
        ConnectionString = _mySqlContainer?.GetConnectionString()
                           ?? Environment.GetEnvironmentVariable("CONNECTION_STRING")!;
    }

    private async Task CreateDatabase()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        SharedDbContext = new AppDbContext(optionsBuilder.Options);
        await SharedDbContext.Database.EnsureDeletedAsync();
        await SharedDbContext.Database.MigrateAsync();
    }

    private async Task SetupRespawner()
    {
        _dbConnection = new MySqlConnection(ConnectionString);
        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.MySql,
                SchemasToInclude = [DbName]
            });
    }
}
