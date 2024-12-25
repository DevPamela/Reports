using Reports.Domain.Entities;

namespace Reports.Application.Models
{
    public class StudentViewModel
    {
        public List<Student>? Students { get; set; }
        public Student Student { get; set; }

    }
}
