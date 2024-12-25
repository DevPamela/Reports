using System.ComponentModel.DataAnnotations;
using Reports.Application.Validators;

namespace Reports.Application.Models
{
    public class RegisterUserViewModel
    {
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [PasswordValidation(ErrorMessage = "A senha deve ter pelo menos 6 caracteres, incluir letras, números e um caractere especial, e não pode conter o nome de usuário.")]
        public string Password { get; set; } = string.Empty;
    }
}
