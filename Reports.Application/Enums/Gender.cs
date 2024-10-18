using System.ComponentModel.DataAnnotations;

namespace Reports.Application.Enums;

public enum Gender
{
    [Display(Name = "Masculino")]
    Male = 0,
    [Display(Name = "Feminino")]
    Female = 1
}