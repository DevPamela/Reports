using Reports.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reports.Domain.Models
{
    [Table("Student")]
    public class Student
    { 
        public School ?School { get; set; }
        public Report? Report { get; set; }
        [Column("Id")]
        [Display(Name = "Code")]
        public Guid Id { get; set; }

        [Column("Name")]
        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Column("Birthday")]
        [Display(Name = "Birthday")]
        public string? Birthday { get; set; }
        public Teacher? Teacher { get; set; }
        public ClassRoom? Class { get; set; }
        public string ?Genre { get; set; }

        [Column("Absences")]
        [Display(Name = "Absences")]
        public string? Absences { get; set; }

        [Column("Goals")]
        [Display(Name = "Goals")]
        public List<Goal>? Goals { get; set; } = new();

        /// <summary>
        /// Validates each objective, setting whether it has been achieved.
        /// </summary>
        //public void IsSatisfatory()
        //{
        //    foreach (var g in Goals)
        //    {
        //        if (string.IsNullOrEmpty(g.Name)) continue;

        //        g.IsAtended();
        //    }
        //}

        /// <summary>
        /// Check that the student is female in order to assign the pronouns correctly in the report.
        /// </summary>
        /// <returns></returns>
        public bool IsAGirl()
        {
            if (Genre == "F")
            {
                return true;
            }
            else if (Genre == "M")
            {
                return false;
            }
            else
                throw new GenreNotFoundException("Invalid gender, please select F for female and M for male.");
          
        }
    }
}
