using FluentResults;
using NPOI.SS.Util;

namespace Reports.Service.ExcelParser.Validators.StudentBirthdayColumn;

/// <summary>
/// Represents the validation errors related to the Student Birthday column.
/// </summary>
public class StudentBirthdayColumnValidationErrors : Error
{
    private StudentBirthdayColumnValidationErrors(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="StudentBirthdayColumnValidationErrors"/> for the case when the cell has an invalid type.
    /// </summary>
    /// <param name="cellAddress">The address of the cell.</param>
    /// <returns>A new instance of the <see cref="StudentBirthdayColumnValidationErrors"/> class.</returns>
    public static StudentBirthdayColumnValidationErrors InvalidType(CellAddress cellAddress) =>
        new($"Data de Nascimento na célula {cellAddress} deve ser do tipo Data.");
}
