using Bogus;
using Bogus.DataSets;
using FluentValidation.TestHelper;
using Reports.Application.Enums;
using Reports.Application.Models;
using Reports.Application.Validators;

namespace Reports.Application.Tests.Validators;

public class AddStudentValidatorTests
{
    private readonly AddStudentValidator _validator = new();
    private readonly Faker<AddStudentViewModel> _requestFaker = new();

    public AddStudentValidatorTests()
    {
        _requestFaker.CustomInstantiator(f =>
        {
            var gender = f.PickRandom(Enum.GetValues<Gender>());

            return new AddStudentViewModel(
                f.Name.FullName(gender == Gender.Female ? Name.Gender.Female : Name.Gender.Male),
                DateOnly.FromDateTime(f.Person.DateOfBirth),
                gender,
                f.Random.Number(1, 100),
                []);
        });
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_ShouldHaveError_WhenNameIsEmpty(string? name)
    {
        // Arrange
        var viewModel = _requestFaker.Generate() with { Name = name };

        // Act
        var result = _validator.TestValidate(viewModel);

        // Assert
        result.ShouldHaveValidationErrorFor(vm => vm.Name)
            .WithErrorMessage("Nome é obrigatório.");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenNameExceedsMaxLength()
    {
        // Arrange
        var viewModel = _requestFaker.Generate() with { Name = new string('a', 121) };

        // Act
        var result = _validator.TestValidate(viewModel);

        // Assert
        result.ShouldHaveValidationErrorFor(vm => vm.Name)
            .WithErrorMessage("Nome deve ter até 120 caracteres.");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenBirthdateIsNull()
    {
        // Arrange
        var viewModel = _requestFaker.Generate() with { Birthdate = null };

        // Act
        var result = _validator.TestValidate(viewModel);

        // Assert
        result.ShouldHaveValidationErrorFor(vm => vm.Birthdate)
            .WithErrorMessage("Data de nascimento é obrigatória.");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenGenderIsNull()
    {
        // Arrange
        var viewModel = _requestFaker.Generate() with { Gender = null };

        // Act
        var result = _validator.TestValidate(viewModel);

        // Assert
        result.ShouldHaveValidationErrorFor(vm => vm.Gender)
            .WithErrorMessage("Gênero é obrigatório.");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenClassroomIdIsNull()
    {
        // Arrange
        var viewModel = _requestFaker.Generate() with { ClassroomId = null };

        // Act
        var result = _validator.TestValidate(viewModel);

        // Assert
        result.ShouldHaveValidationErrorFor(vm => vm.ClassroomId)
            .WithErrorMessage("Turma é obrigatória.");
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenViewModelIsValid()
    {
        // Arrange
        var viewModel = _requestFaker.Generate();

        // Act
        var result = _validator.TestValidate(viewModel);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
