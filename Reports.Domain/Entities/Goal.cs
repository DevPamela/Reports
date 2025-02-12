using Reports.Domain.Interfaces.Model;
using Reports.Domain.Models;

namespace Reports.Domain.Entities
{
    public class Goal : ISelectable
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public List<Student> Students { get; set; } = new();
        public List<Assessment> Assessments { get; set; } = new();
    }
}