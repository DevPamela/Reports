using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reports.Infrastructure;
using Reports.Infrastructure.DbContexts;
using Reports.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

builder.Services.AddServiceLayer();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddAuthentication(options =>
{
    // Set the default schemes
    options.DefaultAuthenticateScheme = "CookieAuthentication"; // Default to cookies
    options.DefaultChallengeScheme = "CookieAuthentication";
})
.AddCookie("CookieAuthentication", options =>
{
    options.LoginPath = "/User/Login"; // Redirect unauthenticated users here
    options.AccessDeniedPath = "/User/AccessDenied"; // Redirect unauthorized users here
})
.AddJwtBearer("Bearer", options =>
{
    options.Authority = builder.Configuration["OpenIddict:Authority"];
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["OpenIddict:Issuer"],
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            // Apply migrations directly
            dbContext.Database.Migrate();
            Console.WriteLine("Migrations applied successfully.");
        }
        catch (Exception ex)
        {
            // Log any migration errors
            Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
        }
    }

    app.UseDeveloperExceptionPage();
}

// Seed OpenIddict during application startup.
await Reports.Infrastructure.Seeders.OpenIddictSeeder.SeedAsync(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
    app.UseExceptionHandler("/Home/Error");
}

app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program; // This is needed for integration tests.
