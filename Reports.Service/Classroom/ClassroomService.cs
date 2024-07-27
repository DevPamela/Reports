using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Models;

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

            await AddClassroom(classroom);
        }
        public async Task AddClassroom(ClassRoom classRoom)
        {
            await ClearIsSelectedProperty();

            if (classRoom.Name == string.Empty != null)
            {
                _classroomRepository.Add(classRoom);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ClassRoom> GetById(int id) => await _classroomRepository.GetByIdAsync(id);
        public async Task<List<ClassRoom>> GetAllAsync() => await _classroomRepository.GetAllAsync();
        public async Task SetClassroomAsTrue(ClassRoom classroom)
        {
            await ClearIsSelectedProperty();
            SelectableEntity<ClassRoom> selectableSomeEntity = new SelectableEntity<ClassRoom>();
            selectableSomeEntity.SetSelectionAsTrue(classroom);

            await _unitOfWork.SaveChangesAsync();
        }
        private async Task ClearIsSelectedProperty()
        {
            SelectableEntity<ClassRoom> selectableSomeEntity = new SelectableEntity<ClassRoom>();

            var classes = await _classroomRepository.GetAllAsync();
            foreach(var classroom in classes)
            {
                selectableSomeEntity.ClearSelection(classroom);
            }
        }
    }
}
