using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io.Dom;
using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Reports.Application.Tests.Fixtures;
using Reports.Infrastructure.DbContexts;
using Reports.Tests.TestHelpers;
using System.Net;

namespace Reports.Application.Tests;

[Collection(nameof(IntegrationCollection))]
public class StudentControllerTests
        : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private const string StudentControllerRoute = "/Student";
    private const string FilePath = "Files";
    private const string RegisterStudentsFormId = "#register-students-form";
    private const string ClassroomNameInputId = "#classroom-name-input";
    private const string ExcelInputId = "#excel-input";
    private const string SubmitButtonId = "#submit-button";
    private const string ValidationErrorsListItem = "#validation-errors ul li";

    private readonly Faker _faker = new();
    private readonly HttpClient _client;
    private readonly SharedFixture _sharedFixture;
    private readonly AppDbContext _dbContext;

    public StudentControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.HttpClient;
        _sharedFixture = factory.SharedFixture;
        _dbContext = _sharedFixture.DbContext!;
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
    public async Task SubmitRegisterStudentsForm_ShouldReturnErrors_WhenFormIsInvalid()
    {
        // Arrange
        var indexPage = await _client.GetAsync(StudentControllerRoute);
        var indexContent = await HtmlHelpers.GetDocumentAsync(indexPage);

        var form = indexContent.QuerySelector<IHtmlFormElement>(RegisterStudentsFormId)!;
        var submitButton = form.QuerySelector<IHtmlInputElement>(SubmitButtonId)!;

        //Act
        var response = await _client.SubmitFormAsync(form, submitButton);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await HtmlHelpers.GetDocumentAsync(response);
        var errors = responseContent.QuerySelectorAll<IHtmlListItemElement>(
            ValidationErrorsListItem).ToList();

        errors.Should().AllSatisfy(e => e.TextContent.Should().NotBeNullOrWhiteSpace());
    }

    [Fact]
    public async Task SubmitRegisterStudentsForm_ShouldReturnErrors_WhenExcelContentIsInvalid()
    {
        // Arrange
        var indexPage = await _client.GetAsync(StudentControllerRoute);
        var indexContent = await HtmlHelpers.GetDocumentAsync(indexPage);

        var form = indexContent.QuerySelector<IHtmlFormElement>(RegisterStudentsFormId)!;
        var submitButton = form.QuerySelector<IHtmlInputElement>(SubmitButtonId)!;

        var fileName = "invalid-excel-input.xlsx";
        using var helper = new FileStreamHelper(FilePath, fileName);

        FillRegisterStudentsForm(
            form, _faker.Random.Word(), new FileEntry(fileName, string.Empty, helper.FileStream));

        //Act
        var response = await _client.SubmitFormAsync(form, submitButton);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await HtmlHelpers.GetDocumentAsync(response);
        var errors = responseContent.QuerySelectorAll<IHtmlListItemElement>(
            ValidationErrorsListItem).ToList();

        errors.Should().AllSatisfy(e => e.TextContent.Should().NotBeNullOrWhiteSpace());
    }

    [Fact]
    public async Task SubmitRegisterStudentsForm_ShouldRedirectToSuccess_WhenFormIsValid()
    {
        // Arrange
        var indexPage = await _client.GetAsync(StudentControllerRoute);
        var content = await HtmlHelpers.GetDocumentAsync(indexPage);

        var form = content.QuerySelector<IHtmlFormElement>(RegisterStudentsFormId)!;
        var submitButton = form.QuerySelector<IHtmlInputElement>(SubmitButtonId)!;

        var fileName = "excel-input.xlsx";
        using var helper = new FileStreamHelper(FilePath, fileName);

        FillRegisterStudentsForm(
            form, _faker.Random.Word(), new FileEntry(fileName, string.Empty, helper.FileStream));

        //Act
        var response = await _client.SubmitFormAsync(form, submitButton);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);

        var classroom = await _dbContext.Classroom.FirstAsync(c => c.TeacherId == _sharedFixture.Teacher.Id);
        response.Headers.Location!.OriginalString.Should().Be($"{StudentControllerRoute}/Success/{classroom.Id}");
    }

    [Fact]
    public async Task SubmitRegisterStudentsForm_ShouldSaveClassroomAndStudentsInDb_WhenFormIsValid()
    {
        // Arrange
        var indexPage = await _client.GetAsync(StudentControllerRoute);
        var content = await HtmlHelpers.GetDocumentAsync(indexPage);

        var form = content.QuerySelector<IHtmlFormElement>(RegisterStudentsFormId)!;
        var submitButton = form.QuerySelector<IHtmlInputElement>(SubmitButtonId)!;

        var classroomName = _faker.Random.Word();
        var fileName = "excel-input.xlsx";
        using var helper = new FileStreamHelper(FilePath, fileName);

        FillRegisterStudentsForm(
            form, classroomName, new FileEntry(fileName, string.Empty, helper.FileStream));

        //Act
        var response = await _client.SubmitFormAsync(form, submitButton);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);

        var classroom = await _dbContext.Classroom.FirstAsync(c => c.TeacherId == _sharedFixture.Teacher.Id);
        classroom.Name.Should().Be(classroomName);

        var students = await _dbContext.Students.Where(s => s.ClassRoomId == classroom.Id).ToListAsync();
        students.Should().HaveCount(1);
        students[0].Name.Should().Be("John Doe");
        students[0].Birthday.Should().Be(new DateTime(2012, 12, 21, 0, 0, 0, DateTimeKind.Local));
    }

    private void FillRegisterStudentsForm(IHtmlFormElement form, string classroomName, IFile file)
    {
        var classroomNameInput = form.QuerySelector<IHtmlInputElement>(ClassroomNameInputId)!;
        classroomNameInput.Value = classroomName;

        var excelInput = form.QuerySelector<IHtmlInputElement>(ExcelInputId)!;
        excelInput.Files!.Add(file);
    }
}
