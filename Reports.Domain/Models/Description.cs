using Reports.Domain.Entities;
using Reports.Domain.Enums;

namespace Reports.Domain.Models
{
    public class Description
    {
        public string AtendedDescription { get; set; } = string.Empty;
        public string ParciallyAtendedDescription { get; set; } = string.Empty;
        public string UnatendedDescription { get; set; } = string.Empty;
        public int GoalId { get; set; }
        public string Gender { get; set; }= string.Empty;
        public Goal Goal { get; set; }
    }
}