using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reports.Application.Middlewares;
using Reports.Application.Models.Configuration;
using Reports.Infrastructure;
using Reports.Infrastructure.DbContexts;
using Reports.Service;
using Serilog;

namespace Reports.Application
{
    public static class ApplicationConfiguration
    {
        /// <summary>
        /// Configures logging for the application.
        /// </summary>
        public static void ConfigureLogger(WebApplicationBuilder builder)
        {
            var basePath = Path.Combine(AppContext.BaseDirectory, "logs");
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            // Read the custom output template from configuration
            var outputTemplate = builder.Configuration["Serilog:CustomSettings:OutputTemplate"]
                ?? throw new ArgumentNullException("OutputTemplate", "Serilog Output Template configuration is missing."); ;

            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(builder.Configuration)
               .Enrich.FromLogContext()
               .Enrich.WithCorrelationId()
               .Enrich.WithThreadId()
               .WriteTo.Console(outputTemplate: outputTemplate)
               .WriteTo.File(
                   Path.Combine(basePath, "log-.txt"),
                   rollingInterval: RollingInterval.Day,
                   outputTemplate: outputTemplate
               )
               .CreateLogger();
            
            builder.Host.UseSerilog();
            builder.Logging.ClearProviders();

            // Configure Microsoft's Logging to use Serilog
            builder.Logging.ClearProviders(); // Clear default providers
            builder.Logging.AddSerilog(Log.Logger);
        }

        /// <summary>
        /// Configures services for the application.
        /// </summary>
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            var sessionConfig = configuration.GetSection("Session").Get<SessionConfig>()
                ?? throw new ArgumentNullException(nameof(SessionConfig), "Session configuration is missing.");

            var openIddictConfig = configuration.GetSection("OpenIddict").Get<OpenIddictConfig>()
                ?? throw new ArgumentNullException(nameof(OpenIddictConfig), "OpenIddict configuration is missing.");

            var authenticationConfig = configuration.GetSection("Authentication").Get<AuthenticationPaths>()
                ?? throw new ArgumentNullException(nameof(AuthenticationPaths), "Authentication configuration is missing.");

            // Register configurations
            builder.Services.Configure<SessionConfig>(configuration.GetSection("Session"));
            builder.Services.Configure<OpenIddictConfig>(configuration.GetSection("OpenIddict"));
            builder.Services.Configure<AuthenticationPaths>(configuration.GetSection("Authentication"));

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });

            // Add session services
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(sessionConfig.IdleTimeoutMinutes);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Configure OpenIddict
            builder.Services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                           .UseDbContext<AppDbContext>();
                })
                .AddServer(options =>
                {
                    options.SetAuthorizationEndpointUris(openIddictConfig.AuthorizationEndpoint)
                           .SetTokenEndpointUris(openIddictConfig.TokenEndpoint)
                           .SetLogoutEndpointUris(openIddictConfig.LogoutEndpoint);

                    options.AllowAuthorizationCodeFlow()
                           .UseReferenceAccessTokens()
                           .UseReferenceRefreshTokens();

                    options.RegisterScopes("openid", "profile", "email");
                    options.SetAccessTokenLifetime(TimeSpan.FromMinutes(openIddictConfig.AccessTokenLifetimeMinutes));

                    options.AddEphemeralEncryptionKey()
                           .AddEphemeralSigningKey();
                });

            // Configure cookie-based authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = authenticationConfig.LoginPath;
                    options.LogoutPath = authenticationConfig.LogoutPath;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(sessionConfig.IdleTimeoutMinutes);
                    options.SlidingExpiration = true;
                });

            builder.Services.AddAuthorization();




            builder.Services.AddServiceLayer();
            builder.Services.AddInfrastructure(configuration);

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
           
        }

        /// <summary>
        /// Configures the database, applies migrations, and seeds data.
        /// </summary>
        public static void ConfigureDatabase(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    dbContext.Database.Migrate();
                    Console.WriteLine("Migrations applied successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
                }

                app.UseDeveloperExceptionPage();
            }

            // Seed OpenIddict during application startup
            Reports.Infrastructure.Seeders.OpenIddictSeeder.SeedAsync(app.Services).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Configures middleware for the application.
        /// </summary>
        public static void ConfigureMiddlewares(WebApplication app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<SessionExpirationMiddleware>();
            app.UseMiddleware<NotificationMiddleware>();
        }

        /// <summary>
        /// Configures the HTTP request pipeline.
        /// </summary>
        public static void ConfigurePipeline(WebApplication app)
        {
            try
            {
                if (!app.Environment.IsDevelopment())
                {
                    app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
                    app.UseExceptionHandler("/Home/Error");
                }

                app.UseStaticFiles();
                app.UseRouting();
                app.UseSession();
                app.UseMiddleware<CorrelationIdMiddleware>();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                app.MapControllers();

            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "An error occurred during pipeline configuration.");
                throw;
            }
        }

    }
}
