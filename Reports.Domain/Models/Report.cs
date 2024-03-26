using System.ComponentModel.DataAnnotations;

namespace Reports.Domain.Models
{
    public class Report
    {
        [Key]
        public string Name  { get; set; }
        public List<Student> Students { get; set; } = new();
        public string? FirtsParagraph { get; set; }
        public string? LastParagraph { get; set; }
    }
}
