using FluentValidation;
using Reports.Application.ViewModels.Student;

namespace Reports.Application.Validators;

/// <summary>
/// Validator for registering students from a file.
/// </summary>
public class RegisterStudentsFromFileValidator
    : AbstractValidator<RegisterStudentsFromFileViewModel>
{
    private const int MaxClassroomNameLength = 80;
    private const int MaxFileSizeInKb = 100;
    private const int MaxFileSizeInBytes = MaxFileSizeInKb * 1024;
    private const string OldExcelExtension = ".xls";
    private const string NewExcelExtension = ".xlsx";

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterStudentsFromFileValidator"/> class.
    /// </summary>
    public RegisterStudentsFromFileValidator()
    {
        RuleFor(vm => vm.ClassroomName)
            .NotEmpty()
            .WithMessage("Nome da turma é obrigatório.")
            .MaximumLength(MaxClassroomNameLength)
            .WithMessage($"Nome da turma deve ter até {MaxClassroomNameLength} caracteres.");

        RuleFor(vm => vm.ExcelFile)
            .NotNull()
            .WithMessage("É necessário anexar um arquivo.")
            .DependentRules(() =>
            {
                RuleFor(vm => vm.ExcelFile.FileName)
                    .Must(fn =>
                        fn.EndsWith(NewExcelExtension, StringComparison.OrdinalIgnoreCase)
                        || fn.EndsWith(OldExcelExtension, StringComparison.OrdinalIgnoreCase))
                    .WithMessage("Tipo de arquivo não permitido.");

                RuleFor(vm => vm.ExcelFile.Length)
                    .LessThanOrEqualTo(MaxFileSizeInBytes)
                    .WithMessage($"Arquivo deve ter até {MaxFileSizeInKb} KB.");
            });
    }
}
