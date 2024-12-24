using System.ComponentModel.DataAnnotations;

namespace Reports.Application.Models
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "O email é obrigatório.")]
        public string email { get; set; } = string.Empty;
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
