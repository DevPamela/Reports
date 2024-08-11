using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Reports.Infrastructure.DbContexts;
using Xunit;

namespace Reports.Tests.TestHelpers;

/// <summary>
/// Represents a custom web application factory for testing purposes.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    /// <summary>
    /// Gets the shared fixture used by the custom web application factory.
    /// </summary>
    public SharedFixture SharedFixture { get; }

    /// <summary>
    /// Gets HTTP client created by the custom web application factory.
    /// </summary>
    public HttpClient HttpClient { get; private set; } = default!;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomWebApplicationFactory"/> class with the specified shared fixture.
    /// </summary>
    /// <param name="fixture">The shared fixture.</param>
    public CustomWebApplicationFactory(SharedFixture fixture)
    {
        SharedFixture = fixture;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            services.RemoveAll(typeof(AppDbContext));

            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    SharedFixture.ConnectionString,
                    ServerVersion.AutoDetect(SharedFixture.ConnectionString)));
        });
    }

    /// <inheritdoc />
    public Task InitializeAsync()
    {
        HttpClient = CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
    }
}
