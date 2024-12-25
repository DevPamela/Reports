using Reports.Domain.Entities;

namespace Reports.Application.Models
{
    public class HomeModel
    {
        public List<ClassRoom> Classrooms { get; set; }

        public bool HasClassRoom { get; set; }

        public Teacher Teacher { get; set; }
    }
}
