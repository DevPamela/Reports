using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Reports.Application.Validators
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;
            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult("A senha é obrigatória.");
            }

            // Obter o Username do modelo
            var usernameProperty = validationContext.ObjectType.GetProperty("Username");
            var username = usernameProperty?.GetValue(validationContext.ObjectInstance, null) as string;

            // Verificar comprimento mínimo
            if (password.Length < 6)
            {
                return new ValidationResult("A senha deve ter pelo menos 6 caracteres.");
            }

            // Verificar se contém letras e números
            if (!Regex.IsMatch(password, @"[A-Za-z]") || !Regex.IsMatch(password, @"\d"))
            {
                return new ValidationResult("A senha deve incluir letras e números.");
            }

            // Verificar se contém um caractere especial
            if (!Regex.IsMatch(password, @"[\W_]"))
            {
                return new ValidationResult("A senha deve incluir pelo menos um caractere especial.");
            }

            // Verificar se contém o Username
            if (!string.IsNullOrEmpty(username) && password.Contains(username, StringComparison.OrdinalIgnoreCase))
            {
                return new ValidationResult("A senha não pode conter o nome de usuário.");
            }

            return ValidationResult.Success;
        }
    }
}
