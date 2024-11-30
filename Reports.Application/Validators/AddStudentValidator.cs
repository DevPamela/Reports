using FluentValidation;
using Reports.Application.Models;
using Reports.Domain.Entities;

namespace Reports.Application.Validators;

/// <summary>
/// Validator for adding students.
/// </summary>
public class AddStudentValidator : AbstractValidator<AddStudentViewModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddStudentValidator"/> class.
    /// </summary>
    public AddStudentValidator()
    {
        RuleFor(vm => vm.Name)
            .NotEmpty()
            .WithMessage("Nome é obrigatório.")
            .MaximumLength(Student.NameMaxLength)
            .WithMessage($"Nome deve ter até {Student.NameMaxLength} caracteres.");

        RuleFor(vm => vm.Birthdate)
            .NotNull()
            .WithMessage("Data de nascimento é obrigatória.");

        RuleFor(vm => vm.Gender)
            .NotNull()
            .WithMessage("Gênero é obrigatório.");

        RuleFor(vm => vm.ClassroomId)
            .NotNull()
            .WithMessage("Turma é obrigatória.");
    }
}
