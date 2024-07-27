namespace Reports.Domain.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? FirtsParagraph { get; set; }
        public string? LastParagraph { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
