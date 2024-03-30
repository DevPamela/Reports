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

        [Column("Validation")]
        [Display(Name = "Validation")]
        public string IsSatisfactory { get; set; }

        [Column("Atended")]
        [Display(Name = "Atended")]
        public string AtendedDescription { get; set; }

        [Column("ParciallyAtended")]
        [Display(Name = "ParciallyAtended")]
        public string ParciallyAtendedDescription { get; set; }

        [Column("Unatended")]
        [Display(Name = "Unatended")]
        public string UnatendedDescription { get; set; }



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