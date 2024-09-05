using FluentResults;
using NPOI.SS.Util;

namespace Reports.Service.ExcelParser.Validators.StudentBirthdateColumn;

/// <summary>
/// Represents the validation errors related to the Student Birthdate column.
/// </summary>
public class StudentBirthdateColumnValidationErrors : Error
{
    private StudentBirthdateColumnValidationErrors(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="StudentBirthdateColumnValidationErrors"/> for the case when the cell has an invalid type.
    /// </summary>
    /// <param name="cellAddress">The address of the cell.</param>
    /// <returns>A new instance of the <see cref="StudentBirthdateColumnValidationErrors"/> class.</returns>
    public static StudentBirthdateColumnValidationErrors InvalidType(CellAddress cellAddress) =>
        new($"Data de Nascimento na célula {cellAddress} deve ser do tipo Data.");
}
