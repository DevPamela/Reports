using System.ComponentModel.DataAnnotations;

namespace Reports.Domain.Models
{
    public class Teacher
    {
        [Key]
        public string Name { get; set; }
        public School School { get; set; }
        public List<Student> Students { get; set; }
        public ClassRoom Class { get; set; }
    }
}