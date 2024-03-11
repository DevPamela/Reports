namespace Reports.Domain.Models
{
    public class Report
    {
        public List<Student> Students { get; set; } = new();
        public string FirtsParagraph { get; set; }
        public string LastParagraph { get; set; }
    }
}
