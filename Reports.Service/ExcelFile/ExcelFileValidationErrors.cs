using FluentResults;
using NPOI.SS.Util;

namespace Reports.Service.ExcelFile;

public class ExcelFileValidationErrors : Error
{
    private ExcelFileValidationErrors(string message)
        : base(message)
    {
    }

    public static ExcelFileValidationErrors HeaderIsNull() =>
        new("Cabeçalho inválido: cabeçalho não deve estar vazio.");

    public static ExcelFileValidationErrors ColumnNotFound(string columnName, string columnReference) =>
        new($"Cabeçalho inválido: {columnName} deve deve estar na coluna {columnReference}.");

    public static ExcelFileValidationErrors RowIsNull(int rowNumber) =>
        new($"Linha {rowNumber} está vazia.");

    public static ExcelFileValidationErrors CellIsNull(string columnName, CellAddress cellAddress) =>
        new($"Célula {columnName}({cellAddress}) está vazia.");

    public static ExcelFileValidationErrors ErrorThresholdReached() =>
        new($"Limite de erros atingido.");

    public static ExcelFileValidationErrors StudentDataNotFound() =>
        new("Nenhum dado de aluno foi encontrado na tabela.");
}
