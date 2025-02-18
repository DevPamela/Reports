using NPOI.SS.UserModel;
using Reports.Domain.Entities;
using Reports.Domain.Enums;
using Reports.Service.ExcelParser.Interfaces;

namespace Reports.Service.ExcelParser.Factories;

/// <summary>
/// Represents a factory for creating instances of the Student class.
/// </summary>
public class StudentFactory : IStudentFactory
{
    private readonly IStudentNameColumn _nameColumn;
    private readonly IStudentBirthdateColumn _birthdateColumn;
    private readonly IStudentGenderColumn _genderColumn;

    /// <summary>
    /// Initializes a new instance of the StudentFactory class.
    /// </summary>
    /// <param name="nameColumn">The column information for the student name.</param>
    /// <param name="birthdateColumn">The column information for the student birthdate.</param>
    /// <param name="genderColumn">The column information for the student gender.</param>
    public StudentFactory(
        IStudentNameColumn nameColumn,
        IStudentBirthdateColumn birthdateColumn,
        IStudentGenderColumn genderColumn)
    {
        _nameColumn = nameColumn;
        _birthdateColumn = birthdateColumn;
        _genderColumn = genderColumn;
    }

    /// <inheritdoc />
    public Student CreateStudent(IRow row)
    {
        var name = row.GetCell(_nameColumn.Index).StringCellValue.Trim();
        var formatedName = GetTheFirstNameAndAbreviatedeSecondName(name);
        var birthdate = row.GetCell(_birthdateColumn.Index).DateOnlyCellValue!.Value;
        var gender = row.GetCell(_genderColumn.Index).StringCellValue
            .Equals("Masculino", StringComparison.OrdinalIgnoreCase)
            ? Gender.Male
            : Gender.Female;

        return new Student
        {
            Name = formatedName,
            Birthdate = birthdate,
            Gender = gender,
        };
    }

    private string GetTheFirstNameAndAbreviatedeSecondName(string fullName)
    {
        var ignorar = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        { "de", "da", "das", "do", "dos" };

        var partes = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var primeiroNome = partes[0];

        string segundoNomeAbreviado = "";
        foreach (var parte in partes.Skip(1))
        {
            if (!ignorar.Contains(parte)) 
            {
                segundoNomeAbreviado = parte.Substring(0, 1).ToUpper() + "."; 
                break;
            }
        }

        var nomeFormatado = segundoNomeAbreviado != "" ? $"{primeiroNome} {segundoNomeAbreviado}" : primeiroNome;

        return nomeFormatado;
    }
}