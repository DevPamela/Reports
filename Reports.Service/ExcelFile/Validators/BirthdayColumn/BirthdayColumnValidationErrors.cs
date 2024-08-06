using FluentResults;
using NPOI.SS.Util;

namespace Reports.Service.ExcelFile.Validators.BirthdayColumn;

public class BirthdayColumnValidationErrors : Error
{
    private BirthdayColumnValidationErrors(string message)
        : base(message)
    {
    }

    public static BirthdayColumnValidationErrors InvalidType(CellAddress cellAddress) =>
        new($"Erro na célula {cellAddress}: Data de Nascimento deve ser do tipo Data.");
}
