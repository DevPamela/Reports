using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Entities;

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
        public async Task<Teacher> GetByUserIdAsync(int id) => await _teacherRepository.GetByUserIdAsync(id);
    }
    //getclassroom with teacherId
}
