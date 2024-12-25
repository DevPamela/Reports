using Reports.Domain.Interfaces.Model;

namespace Reports.Domain.Entities
{
    public class ClassRoom : ISelectable
    {
        public const int NameMaxLength = 80;

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public List<Student> Students { get; set; } = [];
        public bool IsSelected { get; set; } = false;
    }
}