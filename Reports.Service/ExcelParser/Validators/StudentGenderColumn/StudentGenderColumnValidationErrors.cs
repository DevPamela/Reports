using FluentResults;
using NPOI.SS.Util;

namespace Reports.Service.ExcelParser.Validators.StudentGenderColumn;

/// <summary>
/// Represents the validation errors realted to the Student Gender column.
/// </summary>
public class StudentGenderColumnValidationErrors : Error
{
    private StudentGenderColumnValidationErrors(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="StudentGenderColumnValidationErrors"/> for the case when the cell has an invalid type error.
    /// </summary>
    /// <param name="cellAddress">The address of the cell.</param>
    /// <returns>A new instance of <see cref="StudentGenderColumnValidationErrors"/>.</returns>
    public static StudentGenderColumnValidationErrors InvalidType(CellAddress cellAddress) =>
        new($"Gênero na célula {cellAddress} deve ser do tipo Texto.");

    /// <summary>
    /// Creates a new instance of the <see cref="StudentGenderColumnValidationErrors"/> for the case when the gender is empty.
    /// </summary>
    /// <param name="cellAddress">The address of the cell.</param>
    /// <returns>A new instance of <see cref="StudentGenderColumnValidationErrors"/>.</returns>
    public static StudentGenderColumnValidationErrors GenderIsEmpty(CellAddress cellAddress) =>
        new($"Erro na célula {cellAddress}: Gênero não deve estar vazio ou em branco.");

    /// <summary>
    /// Creates a new instance of the <see cref="StudentGenderColumnValidationErrors"/> for the case when the gender is invalid.
    /// </summary>
    /// <param name="cellAddress">The address of the cell.</param>
    /// <returns>A new instance of <see cref="StudentGenderColumnValidationErrors"/>.</returns>
    public static StudentGenderColumnValidationErrors GenderIsInvalid(CellAddress cellAddress) =>
        new($"Erro na célula {cellAddress}: Gênero deve ser 'Feminino' ou 'Masculino'.");
}
