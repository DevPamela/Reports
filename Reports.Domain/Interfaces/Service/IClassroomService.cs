using Reports.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Domain.Interfaces.Service
{
    public interface IClassroomService
    {
        Task AddClassroom(ClassRoom classRoom);
        Task SetClassRommInformation(string ClassroomName, List<Student> students, int teacherId);

        Task<ClassRoom> GetById(int id);
        Task<List<ClassRoom>> GetAllAsync();
        Task SetClassroomAsTrue(ClassRoom classroom);
    }
}
