using Reports.Domain.Models;

namespace Reports.Application.Models
{
    public class HomeModel
    {
        public string Name { get; set; }
        public List<ClassRoom> Classrooms { get; set; }
    }
}
