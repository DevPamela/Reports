using Microsoft.Extensions.Options;
using NSubstitute;
using Reports.Application.Settings;
using Testcontainers.MongoDb;

namespace Reports.Infrastructure.Tests;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer =
        new MongoDbBuilder().Build();

    internal MongoDbContext DbContext { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();

        var connectionString = _mongoDbContainer.GetConnectionString();

        var options = Substitute.For<IOptions<ReportsDatabaseSettings>>();
        options.Value.Returns(new ReportsDatabaseSettings()
        {
            ConnectionString = connectionString,
            DatabaseName = "TestReportsDB",
            StudentsCollectionName = "Students",
            GoalsCollectionName = "Goals",
            IsSSL = false,
        });

        DbContext = new MongoDbContext(options);
    }

    public async Task DisposeAsync() =>
        await _mongoDbContainer.DisposeAsync();
}