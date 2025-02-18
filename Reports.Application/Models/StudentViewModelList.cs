using Reports.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Reports.Application.Models
{
    public class StudentViewModelList
    {
        [Required(ErrorMessage ="A lista de alunos é obrigatória")]
        public List<StudentViewModel>? Students { get; set; }

        [Required(ErrorMessage = "Selecione ao menos um aluno")]
        public StudentViewModel Student { get; set; }

    }
}
