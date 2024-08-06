using FluentValidation;
using Reports.Domain.Interfaces.Service;
using Reports.Infrastructure;
using Reports.Service;
using Reports.Service.Assessments;
using Reports.Service.Classroom;
using Reports.Service.ExcelFile;
using Reports.Service.ExcelFile.Factories;
using Reports.Service.ExcelFile.Interfaces;
using Reports.Service.ExcelFile.Validators.BirthdayColumn;
using Reports.Service.ExcelFile.Validators.NameColumn;
using Reports.Service.Interfaces.Service;
using Reports.Service.Students;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IFormatterService, FormatterService>();
builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<IAssessmentService, AssessmentService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<ExcelFileService>();
builder.Services.AddScoped<IStudentFactory, StudentFactory>();

builder.Services.AddScoped<NameColumnValidator>();
builder.Services.AddScoped<IColumnValidator>(sp =>
    sp.GetRequiredService<NameColumnValidator>());
builder.Services.AddScoped<INameColumnInfo>(sp =>
    sp.GetRequiredService<NameColumnValidator>());

builder.Services.AddScoped<BirthdayColumnValidator>();
builder.Services.AddScoped<IColumnValidator>(sp =>
    sp.GetRequiredService<BirthdayColumnValidator>());
builder.Services.AddScoped<IBirthdayColumnInfo>(sp =>
    sp.GetRequiredService<BirthdayColumnValidator>());

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
