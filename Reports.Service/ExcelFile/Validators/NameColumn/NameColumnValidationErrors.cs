using FluentResults;
using NPOI.SS.Util;

namespace Reports.Service.ExcelFile.Validators.NameColumn;

public class NameColumnValidationErrors : Error
{
    private NameColumnValidationErrors(string message)
        : base(message)
    {
    }

    public static NameColumnValidationErrors InvalidType(CellAddress cellAddress) =>
        new($"Erro na célula {cellAddress}: Nome deve ser do tipo Texto.");

    public static NameColumnValidationErrors NameIsEmpty(CellAddress cellAddress) =>
        new($"Erro na célula {cellAddress}: Nome não deve estar vazio ou em branco.");

    public static NameColumnValidationErrors NameIsTooLong(CellAddress cellAddress, int maxLength) =>
        new($"Erro na célula {cellAddress}: Nome deve ter até {maxLength} caracteres.");
}
