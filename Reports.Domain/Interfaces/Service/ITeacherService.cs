using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Domain.Interfaces.Service
{
    public interface ITeacherService
    {
        Task RegisterTeacher(Teacher teacher);
        Task<Teacher> GetByIdAsync(int id);
    }
}

