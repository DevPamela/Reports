using Bogus;
using FluentValidation.TestHelper;
using Reports.Application.Validators;
using Reports.Application.ViewModels.Student;
using Reports.Tests.TestHelpers;

namespace Reports.Application.Tests.Validators;

public class RegisterStudentsFromListValidatorTests
{
    private const string ExcelExtension = "xlsx";
    private readonly Faker _faker = new();
    private readonly RegisterStudentsFromFileValidator _validator = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_ShouldHaveError_WhenClassroomNameIsEmpty(string? classroomName)
    {
        // Arrange
        using var helper = new FormFileHelper(_faker.System.FileName(ExcelExtension));
        var viewModel = new RegisterStudentsFromFileViewModel(classroomName!, helper.FormFile);

        // Act
        var result = _validator.TestValidate(viewModel);

        // Assert
        result.ShouldHaveValidationErrorFor(vm => vm.ClassroomName)
            .WithErrorMessage("Nome da turma é obrigatório.");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenClassroomNameExceedsMaxLength()
    {
        // Arrange
        using var helper = new FormFileHelper(_faker.System.FileName(ExcelExtension));
        var viewModel = new RegisterStudentsFromFileViewModel(new string('a', 81), helper.FormFile);

        // Act
        var result = _validator.TestValidate(viewModel);

        // Assert
        result.ShouldHaveValidationErrorFor(vm => vm.ClassroomName)
            .WithErrorMessage("Nome da turma deve ter até 80 caracteres.");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenExcelFileIsNull()
    {
        // Arrange
        var viewModel = new RegisterStudentsFromFileViewModel(_faker.Random.Word(), null!);

        // Act
        var result = _validator.TestValidate(viewModel);

        // Assert
        result.ShouldHaveValidationErrorFor(vm => vm.ExcelFile)
            .WithErrorMessage("É necessário anexar um arquivo.");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenExcelFileExceedsMaxLength()
    {
        // Arrange
        using var helper = new FormFileHelper(_faker.System.FileName(ExcelExtension), 101 * 1024);
        var viewModel = new RegisterStudentsFromFileViewModel(_faker.Random.Word(), helper.FormFile);

        // Act
        var result = _validator.TestValidate(viewModel);

        // Assert
        result.ShouldHaveValidationErrorFor(vm => vm.ExcelFile.Length)
            .WithErrorMessage("Arquivo deve ter até 100 KB.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_ShouldHaveError_When_ExcelFileHasNoExtension(string? fileName)
    {
        // Arrange
        using var helper = new FormFileHelper(fileName!);
        var viewModel = new RegisterStudentsFromFileViewModel(_faker.Random.Word(), helper.FormFile);

        // Act
        var result = _validator.TestValidate(viewModel);

        // Assert
        result.ShouldHaveValidationErrorFor(vm => vm.ExcelFile)
            .WithErrorMessage("Tipo de arquivo não permitido.");
    }

    [Fact]
    public void Validate_ShouldHaveError_When_ExcelFileHasInvalidExtension()
    {
        // Arrange
        using var helper = new FormFileHelper(_faker.System.CommonFileName());
        var viewModel = new RegisterStudentsFromFileViewModel(_faker.Random.Word(), helper.FormFile);

        // Act
        var result = _validator.TestValidate(viewModel);

        // Assert
        result.ShouldHaveValidationErrorFor(vm => vm.ExcelFile)
            .WithErrorMessage("Tipo de arquivo não permitido.");
    }

    [Fact]
    public void Validate_ShouldHaveError_When_ExcelFileSignatureIsInvalid()
    {
        // Arrange
        using var helper = new FormFileHelper(_faker.System.CommonFileName(ExcelExtension));
        var viewModel = new RegisterStudentsFromFileViewModel(_faker.Random.Word(), helper.FormFile);

        // Act
        var result = _validator.TestValidate(viewModel);

        // Assert
        result.ShouldHaveValidationErrorFor(vm => vm.ExcelFile)
            .WithErrorMessage("Tipo de arquivo não permitido.");
    }

    [Fact]
    public void Validate_ShouldHaveLengthErrorOnly_WhenExcelFileExceedsMaxLengthAndSignatureIsInvalid()
    {
        // Arrange
        using var helper = new FormFileHelper(_faker.System.CommonFileName(), 101 * 1024);
        var viewModel = new RegisterStudentsFromFileViewModel(_faker.Random.Word(), helper.FormFile);

        // Act
        var result = _validator.TestValidate(viewModel);

        // Assert
        result.ShouldHaveValidationErrorFor(vm => vm.ExcelFile.Length)
            .WithErrorMessage("Arquivo deve ter até 100 KB.")
            .Only();
    }

    [Theory]
    [InlineData("excel-sheet.xlsx")]
    [InlineData("excel-sheet-old.xls")]
    public void Validate_ShouldHaveNoErrors_WhenViewModelIsValid(string fileName)
    {
        // Arrange
        using var fileStreamHelper = new FileStreamHelper("Files", "Validators", fileName);
        using var formFileHelper = new FormFileHelper(fileStreamHelper.FileStream, fileName);
        var viewModel = new RegisterStudentsFromFileViewModel(_faker.Random.Word(), formFileHelper.FormFile);

        // Act
        var result = _validator.TestValidate(viewModel);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
