using Reports.Domain.Exceptions;
using System.ComponentModel.Design;

namespace Reports.Domain.Models
{
    public class Student
    {
        public School School { get; set; }
        public string Name { get; set; }
        public string Birthday { get; set; }
        public Teacher Teacher { get; set; }
        public Class Class { get; set; }
        public string Genre { get; set; }
        public List<Goal> Goals { get; set; }

        /// <summary>
        /// Validates each objective, setting whether it has been achieved.
        /// </summary>
        public void IsSatisfatory()
        {
            foreach (var g in Goals)
            {
                if (string.IsNullOrEmpty(g.Name)) continue;

                g.IsAtended();
            }
        }

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
