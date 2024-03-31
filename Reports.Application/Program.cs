using Reports.Domain.Interfaces;
using Reports.Infrastructure;
using Reports.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastrucuture(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IFormatterService, FormatterService>();
builder.Services.AddScoped<IGoalService, GoalService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
