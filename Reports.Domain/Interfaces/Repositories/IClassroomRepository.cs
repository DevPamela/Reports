using Reports.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Domain.Interfaces.Repositories
{
    public interface IClassroomRepository
    {
        void Add(ClassRoom classRoom);

        Task<List<ClassRoom>> GetAllAsync();

        void Update(ClassRoom classRoom);

        Task<ClassRoom?> GetByNameAsync(string name);

        Task<ClassRoom?> GetByIdAsync(int id);
    }
}
