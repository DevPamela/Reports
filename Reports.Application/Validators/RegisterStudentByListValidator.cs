using FluentValidation;
using Reports.Application.ViewModels.Student;

namespace Reports.Application.Validators;

public class RegisterStudentByListValidator
    : AbstractValidator<RegisterStudentByListViewModel>
{
    public RegisterStudentByListValidator()
    {
        RuleFor(vm => vm.ClassroomName)
            .NotEmpty()
            .WithMessage("Nome da turma é obrigatório.")
            .MaximumLength(80)
            .WithMessage("Nome da turma deve ter até 80 caracteres.");

        RuleFor(vm => vm.ExcelFile)
            .NotNull()
            .WithMessage("É necessário anexar um arquivo.")
            .DependentRules(() =>
            {
                RuleFor(vm => vm.ExcelFile.FileName)
                    .Must(fn => fn.EndsWith(".xls") || fn.EndsWith(".xlsx"))
                    .WithMessage("Tipo de arquivo não permitido.");

                RuleFor(vm => vm.ExcelFile.Length)
                    .LessThanOrEqualTo(100 * 1024)
                    .WithMessage("Arquivo deve ter até 100 KB.");
            });
    }
}
