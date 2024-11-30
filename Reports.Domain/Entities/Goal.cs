using Reports.Domain.Interfaces.Model;

namespace Reports.Domain.Entities
{
    public class Goal: ISelectable
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AtendedDescription { get; set; } = string.Empty;
        public bool IsSelected { get; set; } = false;
        public string ParciallyAtendedDescription { get; set; } = string.Empty;
        public string UnatendedDescription { get; set; } = string.Empty;
        //TODO: check nullability of foreign key
        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public List<Student> Students { get; set; } = new();
        public List<Assessment> Assessments { get; set; } = new();
    }
}