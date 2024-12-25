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
        /// <summary>
        /// Add a new Teacher based on User data.
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        Task RegisterTeacher(Teacher teacher);
        /// <summary>
        /// Get the Teacher by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Teacher> GetByIdAsync(int id);

        /// <summary>
        /// Get the Teacher by User id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Teacher> GetByUserIdAsync(int id);
    }
}

