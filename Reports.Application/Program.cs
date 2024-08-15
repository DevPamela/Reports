using FluentValidation;
using Reports.Domain.Interfaces.Service;
using Reports.Infrastructure;
using Reports.Service;
using Reports.Service.Assessments;
using Reports.Service.Classroom;
using Reports.Service.Students;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IFormatterService, FormatterService>();
builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<IAssessmentService, AssessmentService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();

builder.Services.AddServiceLayer();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

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

public partial class Program; // This is needed for integration tests.
