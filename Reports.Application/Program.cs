using Reports.Domain.Interfaces.Service;
using Reports.Infrastructure;
using Reports.Service;
using Reports.Service.Assessments;
using Reports.Service.Classroom;
using Reports.Service.ExcelFileHeader;
using Reports.Service.Students;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IFormatterService, FormatterService>();
builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<IAssessmentService, AssessmentService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IExcelFileHeaderService, ExcelFileHeaderService>();

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
