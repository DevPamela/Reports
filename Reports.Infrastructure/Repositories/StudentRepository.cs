using MongoDB.Driver;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;

namespace Reports.Infrastructure.Repositories
{
    internal class StudentRepository : IStudentRepository
    {
        private readonly MongoDbContext _dbContext;

        public StudentRepository(MongoDbContext dbContext) =>
            _dbContext = dbContext;

        /// <inheritdoc />
        public async Task AddOneAsync(Student student) =>
            await _dbContext.StudentsCollection
                .InsertOneAsync(student);

        /// <inheritdoc />
        public async Task<List<Student>> GetAllAsync() =>
            await _dbContext.StudentsCollection
                .Find(_ => true)
                .ToListAsync();

        /// <inheritdoc />
        public async Task<Student?> GetByNameAsync(string name) =>
            await _dbContext.StudentsCollection
                .Find(s => s.Name == name)
                .FirstOrDefaultAsync();

        /// <inheritdoc />
        public async Task UpdateOneStudentGoalsAsync(string studentName, IEnumerable<Goal> goals)
        {
            var update = Builders<Student>.Update.Set(s => s.Goals, goals);
            await _dbContext.StudentsCollection.UpdateOneAsync(s => s.Name == studentName, update);
        }
    }
}
