namespace Reports.Domain.Models;
public class Assessment
{
    public int StudentId { get; set; }
    public Student? Student { get; set; }
    public int GoalId { get; set; }
    public Goal? Goal { get; set; }
    public string IsSatisfactory { get; set; } = string.Empty;
}
