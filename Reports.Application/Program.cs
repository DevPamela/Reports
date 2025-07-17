using Reports.Application;

var builder = WebApplication.CreateBuilder(args);
// Add Configuration Sources 
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure Logging
ApplicationConfiguration.ConfigureLogger(builder);

// Configure Services
ApplicationConfiguration.ConfigureServices(builder);

var app = builder.Build();
app.UseSession();
// Configure Application
ApplicationConfiguration.ConfigureDatabase(app);
ApplicationConfiguration.ConfigureMiddlewares(app);
ApplicationConfiguration.ConfigurePipeline(app);

app.Run();

public partial class Program; // This is needed for integration tests.
