using Microsoft.EntityFrameworkCore;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Entities;
using Reports.Infrastructure.DbContexts;

namespace Reports.Infrastructure.Repositories
{
    internal class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _dbContext;

        public StudentRepository(AppDbContext dbContext) =>
            _dbContext = dbContext;

        /// <inheritdoc />
        public void Add(Student student) =>
            _dbContext.Students.Add(student);

     

        public async Task<List<Student>> GetStudentByIsActiveAsync(int teacherId) =>
            await _dbContext.Students.Where(_ => _.IsActive == true && _.TeacherId == teacherId).ToListAsync();

        public void DeleteAsync(Student student) =>
             _dbContext.Students.Remove(student);

        public void Update(Student student) =>
       _dbContext.Students.Update(student);


        /// <inheritdoc />
        public async Task<Student?> GetByNameAsync(string name, int teacherId) =>
            await _dbContext.Students
                .FirstOrDefaultAsync(s => s.Name == name & s.TeacherId == teacherId);

        /// <inheritdoc />
        public async Task<Student?> GetByIdAsync(int id) =>
            await _dbContext.Students
                .FirstOrDefaultAsync(s => s.Id == id);

        /// <inheritdoc />
        public async Task<Student?> GetByClassroomIdAsync(int id, int teacherId) =>
       await _dbContext.Students
           .Where(s => s.ClassRoomId == id && s.TeacherId == teacherId)
           .FirstOrDefaultAsync();

        public async Task<List<Student>> GetStudentsSelectedByClasses(IEnumerable<int> selectedClasses, int teacherId) =>
    await _dbContext.Students
        .Where(student => selectedClasses.Contains(student.ClassRoomId) && student.TeacherId == teacherId)
        .ToListAsync();

        public async Task<List<Student>> GetByIsSelectedProperty(int teacherId) =>
         await _dbContext.Students
             .Where(student => student.IsActive == true && student.TeacherId == teacherId)
             .ToListAsync();

        public async Task<List<Student>> GetStudentsByIdListAsync(List<int> ids, int teacherId)
        {
            return await _dbContext.Students
                .Where(student => ids.Contains(student.Id) && student.TeacherId == teacherId)
                .ToListAsync();
        }
        public async Task<List<Student>> GetStudentsByTeacherIdListAsync(int TeacherId)
        {
            return await _dbContext.Students
                .Where(student => student.TeacherId == TeacherId)
                .ToListAsync();
        }
    }
}
