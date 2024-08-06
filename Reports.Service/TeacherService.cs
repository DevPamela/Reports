using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Models;
using Reports.Service.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Service
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IUnitOfWork _unitOfWork;
        public TeacherService(ITeacherRepository teacherRepository, IUnitOfWork unitOfWork)
        {
            _teacherRepository = teacherRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task RegisterTeacher(Teacher teacher)
        {
            _teacherRepository.Add(teacher);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Teacher> GetByIdAsync(int id) => await _teacherRepository.GetByIdAsync(id);
    }
    //getclassroom with teacherId
}
