using System.ComponentModel.DataAnnotations;

namespace Reports.Application.Models
{
    public class ClassroomViewModelList
    {
        [Required(ErrorMessage = "É necessário selecionar pelo menos uma turma.")]
        public List<ClassroomViewModel> Classes { get; set; }

        [Required(ErrorMessage = "Selecione pelo menos uma turma.")]
        public List<int> SelectedClasses { get; set; } = new(); 
    }
}
