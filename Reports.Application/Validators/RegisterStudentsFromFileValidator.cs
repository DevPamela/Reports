using FluentValidation;
using Reports.Application.Models;

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

    private static readonly Dictionary<string, byte[]> FileSignature =
    new()
    {
        { ".xlsx", [0x50, 0x4B, 0x03, 0x04] },
        { ".xls", [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1] }
    };

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
                RuleFor(vm => vm.ExcelFile.Length)
                    .LessThanOrEqualTo(MaxFileSizeInBytes)
                    .WithMessage($"Arquivo deve ter até {MaxFileSizeInKb} KB.")
                    .DependentRules(() =>
                    {
                        RuleFor(vm => vm.ExcelFile)
                            .Must(IsValidFile)
                            .WithMessage("Tipo de arquivo não permitido.");
                    });
            });
    }

    private static bool IsValidFile(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();

        if (string.IsNullOrEmpty(extension)
            || !FileSignature.TryGetValue(extension, out var signature))
        {
            return false;
        }

        using var reader = new BinaryReader(file.OpenReadStream());
        var headerBytes = reader.ReadBytes(signature.Length);

        return headerBytes.SequenceEqual(signature);
    }
}
