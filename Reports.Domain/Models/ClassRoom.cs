using Reports.Domain.Interfaces.Model;

namespace Reports.Domain.Models
{
    public class ClassRoom: ISelectable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public List<Student> Students { get; set; } = new();
        public bool IsSelected { get; set; } = false;
    }
}