using System.ComponentModel.DataAnnotations;

namespace Reports.Application.Models;

/// <summary>
/// Represents a view model for registering students from a file.
/// </summary>

public record RegisterStudentsFromFileViewModel(
    [Required(ErrorMessage = "O nome da turma é obrigatório.")] string ClassroomName, 
    [Required(ErrorMessage = "Selecione um arquivo excel com sua lista de alunos.")] IFormFile ExcelFile);
