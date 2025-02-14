using Reports.Domain.Entities;

namespace Reports.Application.Models
{
    public class ClassroomAndStudentViewModel
    {
        public List<ClassRoom>? Classes { get; set; }
        public Student Student { get; set; }
    }
}
