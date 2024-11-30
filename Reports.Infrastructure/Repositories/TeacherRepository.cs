using Microsoft.EntityFrameworkCore;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Entities;
using Reports.Infrastructure.DbContexts;

namespace Reports.Infrastructure.Repositories;
internal class TeacherRepository : ITeacherRepository
{
    private readonly AppDbContext _dbContext;

    public TeacherRepository(AppDbContext dbContext) =>
        _dbContext = dbContext;

    /// <inheritdoc />
    public void Add(Teacher teacher) =>
        _dbContext.Teacher.Add(teacher);

    public void Update(Teacher teacher) =>
        _dbContext.Teacher.Update(teacher);

    public async Task<Teacher?> GetByIdAsync(int id) =>
          await _dbContext.Teacher
              .FirstOrDefaultAsync(a => a.Id == id);
}
