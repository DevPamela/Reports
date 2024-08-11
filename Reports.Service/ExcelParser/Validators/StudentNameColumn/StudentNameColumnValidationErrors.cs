using FluentResults;
using NPOI.SS.Util;

namespace Reports.Service.ExcelParser.Validators.StudentNameColumn;

/// <summary>
/// Represents the validation errors related to the Student Name column.
/// </summary>
public class StudentNameColumnValidationErrors : Error
{
    private StudentNameColumnValidationErrors(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="StudentNameColumnValidationErrors"/> for the case when the cell has an invalid type.
    /// </summary>
    /// <param name="cellAddress">The address of the cell.</param>
    /// <returns>A new instance of <see cref="StudentNameColumnValidationErrors"/>.</returns>
    public static StudentNameColumnValidationErrors InvalidType(CellAddress cellAddress) =>
        new($"Nome na célula {cellAddress} deve ser do tipo Texto.");

    /// <summary>
    /// Creates a new instance of <see cref="StudentNameColumnValidationErrors"/> for the case when the name is empty.
    /// </summary>
    /// <param name="cellAddress">The address of the cell.</param>
    /// <returns>A new instance of <see cref="StudentNameColumnValidationErrors"/>.</returns>
    public static StudentNameColumnValidationErrors NameIsEmpty(CellAddress cellAddress) =>
        new($"Erro na célula {cellAddress}: Nome não deve estar vazio ou em branco.");

    /// <summary>
    /// Creates a new instance of <see cref="StudentNameColumnValidationErrors"/> for the case when the name is too long.
    /// </summary>
    /// <param name="cellAddress">The address of the cell.</param>
    /// <param name="maxLength">The maximum length of the name.</param>
    /// <returns>A new instance of <see cref="StudentNameColumnValidationErrors"/>.</returns>
    public static StudentNameColumnValidationErrors NameIsTooLong(CellAddress cellAddress, int maxLength) =>
        new($"Erro na célula {cellAddress}: Nome deve ter até {maxLength} caracteres.");
}
