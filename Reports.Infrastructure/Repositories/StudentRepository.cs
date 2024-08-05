using Microsoft.EntityFrameworkCore;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;
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

        public async Task<Student?> GetByIdAsync(int id) =>
            await _dbContext.Students
                .FirstOrDefaultAsync(s => s.Id == id);

    }
}
