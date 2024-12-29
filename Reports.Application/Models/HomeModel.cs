using Reports.Domain.Entities;

namespace Reports.Application.Models
{
    public class HomeModel
    {
        public List<ClassRoom> Classrooms { get; set; }

        public bool HasClassRoom => Classrooms != null && Classrooms.Count > 0;

        public Teacher Teacher { get; set; }
    }
}
