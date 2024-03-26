using System.ComponentModel.DataAnnotations;

namespace Reports.Domain.Models
{
    public class ClassRoom
    {
        public List<Student> Students { get; set; } = new();
        public Teacher Teacher { get; set; }
        public School School { get; set; }
       
        public string Name { get; set; }
        public int Id { get; set; }
    }
}