using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Reports.Domain.Models;
using Reports.Infrastructure.DbContexts;
using Respawn;
using System.Data.Common;
using Testcontainers.MySql;
using Xunit;

namespace Reports.Tests.TestHelpers;

/// <summary>
/// Represents a shared fixture for integration tests.
/// </summary>
public class SharedFixture : IAsyncLifetime
{
    private readonly MySqlContainer _mySqlContainer = new MySqlBuilder().Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    /// <summary>
    /// Gets the connection string for the MySQL container.
    /// </summary>
    public string ConnectionString => _mySqlContainer.GetConnectionString();

    /// <summary>
    /// Gets the instance of the application database context.
    /// </summary>
    public AppDbContext DbContext { get; private set; } = default!;

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
        DbContext.Teacher.Add(Teacher);
        await DbContext.SaveChangesAsync();
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
        await _mySqlContainer.StartAsync();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));

        DbContext = new AppDbContext(optionsBuilder.Options);
        await DbContext.Database.MigrateAsync();

        _dbConnection = new MySqlConnection(ConnectionString);
        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.MySql
            });
    }

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await _dbConnection.DisposeAsync();
        await _mySqlContainer.DisposeAsync();
    }
}
