using Reports.Domain.Exceptions;

namespace Reports.Domain.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Birthday { get; set; }
        public string? Genre { get; set; }
        public string? Absences { get; set; }
        //TODO: check nullability of foreign key
        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        //TODO: check nullability of foreign key
        public int? ClassRoomId { get; set; }
        public ClassRoom? Class { get; set; }
        public List<Report> Reports { get; set; } = new();
        public List<Goal> Goals { get; set; } = new();
        public List<Assessment> Assessments { get; set; } = new();

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
