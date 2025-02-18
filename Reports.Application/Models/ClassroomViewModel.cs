using Reports.Domain.Entities;

namespace Reports.Application.Models
{
    public class ClassroomViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public List<StudentViewModel> Students { get; set; } = [];
    }
}
