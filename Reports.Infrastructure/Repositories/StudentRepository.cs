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

        /// <inheritdoc />
        public async Task<List<Student>> GetAllAsync() =>
            await _dbContext.Students.ToListAsync();

        public void DeleteAsync(Student student) =>
             _dbContext.Students.Remove(student);

        public void Update(Student student) =>
       _dbContext.Students.Update(student);


        /// <inheritdoc />
        public async Task<Student?> GetByNameAsync(string name) =>
            await _dbContext.Students
                .FirstOrDefaultAsync(s => s.Name == name);

        /// <inheritdoc />
        public async Task<Student?> GetByIdAsync(int id) =>
            await _dbContext.Students
                .FirstOrDefaultAsync(s => s.Id == id);

        /// <inheritdoc />
        public async Task<Student?> GetByClassroomIdAsync(int id) =>
       await _dbContext.Students
           .Where(s => s.ClassRoomId == id)
           .FirstOrDefaultAsync();

        public async Task<List<Student>> GetStudentsSelectedByClasses(IEnumerable<int> selectedClasses) =>
    await _dbContext.Students
        .Where(student => selectedClasses.Contains(student.ClassRoomId))
        .ToListAsync();

        public async Task<List<Student>> GetByIsSelectedProperty() =>
         await _dbContext.Students
             .Where(student => student.IsActive == true)
             .ToListAsync();

        public async Task<List<Student>> GetStudentsByIdListAsync(List<int> ids)
        {
            return await _dbContext.Students
                .Where(student => ids.Contains(student.Id))
                .ToListAsync();
        }
    }
}
