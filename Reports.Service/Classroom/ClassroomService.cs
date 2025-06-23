using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Entities;
using Reports.Domain.Entities.Responses;

namespace Reports.Service.Classroom
{
    public class ClassroomService : IClassroomService
    {
        private readonly IClassroomRepository _classroomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ClassroomService(IClassroomRepository classroomRepository, IUnitOfWork unitOfWork)
        {
            _classroomRepository = classroomRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task SetClassRommInformation(string ClassroomName, List<Student> students, int teacherId)
        {
            var classroom = new ClassRoom
            {
                Name = ClassroomName,
                Students = students,
                TeacherId = teacherId
            };

            await AddClassroom(classroom,  teacherId);
        }
        public async Task AddClassroom(ClassRoom classRoom, int teacherId)
        {
            await ClearIsSelectedProperty(teacherId);

            if (classRoom.Name == string.Empty != null)
            {
                _classroomRepository.Add(classRoom);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<ClassRoom>> GetByTeacherId(int id) => await _classroomRepository.GetClassroomByTeacherId(id);
        public async Task<ClassRoom> GetById(int id) => await _classroomRepository.GetByIdAsync(id);
        public async Task<List<ClassRoom>> GetAllAsync(int teacherId) => await _classroomRepository.GetAllAsync(teacherId);
        public async Task SetClassroomAsTrue(ClassRoom classroom, int teacherId)
        {
            await ClearIsSelectedProperty(teacherId);
            SelectableEntity<ClassRoom> selectableSomeEntity = new SelectableEntity<ClassRoom>();
            selectableSomeEntity.SetSelectionAsTrue(classroom);

            await _unitOfWork.SaveChangesAsync();
        }
        private async Task ClearIsSelectedProperty(int teacherId)
        {
            SelectableEntity<ClassRoom> selectableSomeEntity = new SelectableEntity<ClassRoom>();

            var classes = await _classroomRepository.GetAllAsync(teacherId);
            foreach(var classroom in classes)
            {
                selectableSomeEntity.ClearSelection(classroom);
            }
        }
    }
}
