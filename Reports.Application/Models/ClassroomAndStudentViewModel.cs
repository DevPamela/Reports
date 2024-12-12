using Reports.Domain.Entities;
using Reports.Domain.Entities.Responses;

namespace Reports.Application.Models
{
    public class ClassroomAndStudentViewModel
    {
        public List<ClassroomNameResponse>? Classes { get; set; }
        public Student Student { get; set; }
    }
}
