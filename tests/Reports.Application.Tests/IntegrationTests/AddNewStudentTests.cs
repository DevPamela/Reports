using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Reports.Application.Enums;
using Reports.Application.Tests.Fixtures;
using Reports.Infrastructure.DbContexts;
using Reports.Tests.TestHelpers.IntegrationTesting;
using Reports.Tests.TestHelpers.Web;
using System.Net;

namespace Reports.Application.Tests.IntegrationTests;

[Collection(nameof(IntegrationCollection))]
public class AddNewStudentTests
    : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private const string AddNewStudentRoute = "/Student/AddNewStudent";
    private const string AddNewStudentFormId = "#add-new-student-form";
    private const string NameInputId = "#name";
    private const string BirthdateInputId = "#birthdate";
    private const string GenderSelectId = "#gender";
    private const string ClassroomSelectId = "#classroom";
    private const string SubmitButtonId = "#submit-button";

    private readonly HttpClient _client;
    private readonly SharedFixture _sharedFixture;
    private readonly AppDbContext _sharedDbContext;

    public AddNewStudentTests(CustomWebApplicationFactory factory)
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
    public async Task SubmitAddNewStudentForm_ShouldAddStudent_WhenFormIsValid()
    {
        // Arrange
        var page = await _client.GetAsync(AddNewStudentRoute);
        var content = await HtmlHelpers.GetDocumentAsync(page);

        var form = content.QuerySelector<IHtmlFormElement>(AddNewStudentFormId)!;
        var submitButton = form.QuerySelector<IHtmlInputElement>(SubmitButtonId)!;

        FillAddNewStudentForm(form);

        //Act
        var response = await _client.SubmitFormAsync(form, submitButton);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);

        var students = await _sharedDbContext.Students
            .Where(s => s.ClassRoomId == _sharedFixture.Classroom.Id && s.TeacherId == _sharedFixture.Teacher.Id)
            .ToListAsync();

        students.Should().HaveCount(1);
        students[0].Name.Should().Be("John Doe");
        students[0].Birthdate.Should().Be(new DateOnly(2012, 12, 21));
        students[0].Gender.Should().Be(Domain.Enums.Gender.Male);
    }

    private void FillAddNewStudentForm(IHtmlFormElement form)
    {
        var nameInput = form.QuerySelector<IHtmlInputElement>(NameInputId)!;
        nameInput.Value = "John Doe";

        var birthdateInput = form.QuerySelector<IHtmlInputElement>(BirthdateInputId)!;
        birthdateInput.Value = new DateOnly(2012, 12, 21).ToString("yyyy-MM-dd");

        var genderSelect = form.QuerySelector<IHtmlSelectElement>(GenderSelectId)!;
        genderSelect.Value = ((int)Gender.Male).ToString();

        var classroomSelect = form.QuerySelector<IHtmlSelectElement>(ClassroomSelectId)!;
        classroomSelect.Value = _sharedFixture.Classroom.Id.ToString();
    }
}
