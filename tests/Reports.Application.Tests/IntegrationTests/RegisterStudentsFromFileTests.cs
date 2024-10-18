using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io.Dom;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Reports.Application.Tests.Fixtures;
using Reports.Domain.Enums;
using Reports.Infrastructure.DbContexts;
using Reports.Tests.TestHelpers.Files;
using Reports.Tests.TestHelpers.IntegrationTesting;
using Reports.Tests.TestHelpers.Web;
using System.Net;

namespace Reports.Application.Tests.IntegrationTests;

[Collection(nameof(IntegrationCollection))]
public class RegisterStudentsFromFileTests
        : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private const string StudentControllerRoute = "/Student";
    private const string FilePath = "Files";
    private const string RegisterStudentsFormId = "#register-students-form";
    private const string ClassroomNameInputId = "#classroom-name-input";
    private const string ExcelInputId = "#excel-input";
    private const string SubmitButtonId = "#submit-button";

    private readonly HttpClient _client;
    private readonly SharedFixture _sharedFixture;
    private readonly AppDbContext _sharedDbContext;

    public RegisterStudentsFromFileTests(CustomWebApplicationFactory factory)
    {
        _client = factory.HttpClient;
        _sharedFixture = factory.SharedFixture;
        _sharedDbContext = _sharedFixture.SharedDbContext;
    }

    public async Task InitializeAsync()
    {
        await _sharedFixture.SeedAsync();
    }

    public async Task DisposeAsync()
    {
        await _sharedFixture.ResetDatabaseAsync();
    }

    [Fact]
    public async Task SubmitRegisterStudentsForm_ShouldSaveClassroomAndStudentsInDb_WhenFormIsValid()
    {
        // Arrange
        var indexPage = await _client.GetAsync(StudentControllerRoute);
        var content = await HtmlHelpers.GetDocumentAsync(indexPage);

        var form = content.QuerySelector<IHtmlFormElement>(RegisterStudentsFormId)!;
        var submitButton = form.QuerySelector<IHtmlInputElement>(SubmitButtonId)!;

        var classroomName = "Test Classroom";
        var fileName = "excel-input.xlsx";
        using var helper = new FileStreamHelper(FilePath, fileName);

        FillRegisterStudentsForm(
            form, classroomName, new FileEntry(fileName, string.Empty, helper.FileStream));

        //Act
        var response = await _client.SubmitFormAsync(form, submitButton);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);

        var classroom = await _sharedDbContext.Classroom
            .FirstAsync(c => c.TeacherId == _sharedFixture.Teacher.Id && c.Name == classroomName);

        var students = await _sharedDbContext.Students
            .Where(s => s.ClassRoomId == classroom.Id && s.TeacherId == _sharedFixture.Teacher.Id)
            .ToListAsync();

        students.Should().HaveCount(1);
        students[0].Name.Should().Be("John Doe");
        students[0].Birthdate.Should().Be(new DateOnly(2012, 12, 21));
        students[0].Gender.Should().Be(Gender.Male);
    }

    private static void FillRegisterStudentsForm(IHtmlFormElement form, string classroomName, IFile file)
    {
        var classroomNameInput = form.QuerySelector<IHtmlInputElement>(ClassroomNameInputId)!;
        classroomNameInput.Value = classroomName;

        var excelInput = form.QuerySelector<IHtmlInputElement>(ExcelInputId)!;
        excelInput.Files!.Add(file);
    }
}
