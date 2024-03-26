using System.ComponentModel.DataAnnotations;

namespace Reports.Domain.Models
{
    public class School
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        //public List<ClassRoom> Classes { get; set; }
        //public int Stamp { get; set; }
        //public List<Teacher> Teachers { get; set; }
    }
}