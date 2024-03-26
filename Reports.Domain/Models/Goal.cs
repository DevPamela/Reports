using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reports.Domain.Models
{
    [Table("Goals")]
    public class Goal
    {
        [Column("Id")]
        [Display(Name = "GoalCode")]
        public Guid Id { get; set; }
        [Column("Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Column("Value")]
        [Display(Name = "Value")]
        public string IsSatisfactory { get; set; }
        public void IsAtended()
        {
            if (IsSatisfactory == "S")
            {
                this.GetGoalsReachedFromDB();
            }
            else if (IsSatisfactory == "PS")
            {
                this.GetGoalsPartiallyReachedFromDB();

            }
            else
            {
                this.GetGoalsNotReachedFromDB();
            }
        }
    }
}