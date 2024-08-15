using FluentResults;
using NPOI.SS.Util;

namespace Reports.Service.ExcelParser;

/// <summary>
/// Represents the validation errors that can occur during Excel parsing.
/// </summary>
public class ExcelParserValidationErrors : Error
{
    private ExcelParserValidationErrors(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="ExcelParserValidationErrors"/> for the case when the header is null.
    /// </summary>
    /// <returns>A new instance of the <see cref="ExcelParserValidationErrors"/> class.</returns>
    public static ExcelParserValidationErrors HeaderIsNull() =>
        new("Cabeçalho inválido: cabeçalho não deve estar vazio.");

    /// <summary>
    /// Creates a new instance of <see cref="ExcelParserValidationErrors"/> for the case when a column is not found.
    /// </summary>
    /// <param name="columnName">The name of the column.</param>
    /// <param name="columnReference">The column reference.</param>
    /// <returns>A new instance of the <see cref="ExcelParserValidationErrors"/> class.</returns>
    public static ExcelParserValidationErrors ColumnNotFound(string columnName, string columnReference) =>
        new($"Cabeçalho inválido: {columnName} deve deve estar na coluna {columnReference}.");

    /// <summary>
    /// Creates a new instance of <see cref="ExcelParserValidationErrors"/> for the case when a row is null.
    /// </summary>
    /// <param name="rowNumber">The number of the row.</param>
    /// <returns>A new instance of the <see cref="ExcelParserValidationErrors"/> class.</returns>
    public static ExcelParserValidationErrors RowIsNull(int rowNumber) =>
        new($"Linha {rowNumber} está vazia.");

    /// <summary>
    /// Creates a new instance of <see cref="ExcelParserValidationErrors"/> for the case when a cell is null.
    /// </summary>
    /// <param name="columnName">The name of the column.</param>
    /// <param name="cellAddress">The address of the cell.</param>
    /// <returns>A new instance of the <see cref="ExcelParserValidationErrors"/> class.</returns>
    public static ExcelParserValidationErrors CellIsNull(string columnName, CellAddress cellAddress) =>
        new($"Célula {columnName}({cellAddress}) está vazia.");

    /// <summary>
    /// Creates a new instance of <see cref="ExcelParserValidationErrors"/> for the case when the error threshold is reached.
    /// </summary>
    /// <returns>A new instance of the <see cref="ExcelParserValidationErrors"/> class.</returns>
    public static ExcelParserValidationErrors ErrorThresholdReached() =>
        new($"Limite de erros atingido.");

    /// <summary>
    /// Creates a new instance of <see cref="ExcelParserValidationErrors"/> for the case when student data is not found in the table.
    /// </summary>
    /// <returns>A new instance of the <see cref="ExcelParserValidationErrors"/> class.</returns>
    public static ExcelParserValidationErrors StudentDataNotFound() =>
        new("Nenhum dado de aluno foi encontrado na tabela.");
}
