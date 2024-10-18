using Microsoft.EntityFrameworkCore;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;
using Reports.Domain.Models.Responses;
using Reports.Infrastructure.DbContexts;

namespace Reports.Infrastructure.Repositories;
public class ClassroomRepository : IClassroomRepository
{
    private readonly AppDbContext _dbContext;

    public ClassroomRepository(AppDbContext dbContext) =>
        _dbContext = dbContext;

    /// <inheritdoc />
    public void Add(ClassRoom classRoom) =>
        _dbContext.Classroom.Add(classRoom);

    /// <inheritdoc />
    public async Task<List<ClassRoom>> GetAllAsync() =>
        await _dbContext.Classroom.ToListAsync();

    /// <inheritdoc />
    public async Task<ClassRoom?> GetByNameAsync(string name) =>
        await _dbContext.Classroom
            .FirstOrDefaultAsync(a => a.Name == name);

    /// <inheritdoc />
    public async Task<ClassRoom?> GetByIdAsync(int id) =>
          await _dbContext.Classroom
              .FirstOrDefaultAsync(a => a.Id == id);

    /// <inheritdoc />
    public async Task<bool> ClassroomForTeacherExists(int classroomId, int teacherId) =>
        await _dbContext.Classroom
            .AnyAsync(c => c.Id == classroomId && c.TeacherId == teacherId);

    /// <inheritdoc />
    public async Task<List<ClassroomNameResponse>> GetClassroomNames(int teacherId)
    {
        return await _dbContext.Classroom
            .Where(c => c.TeacherId == teacherId)
            .Select(c => new ClassroomNameResponse(c.Id, c.Name))
            .ToListAsync();
    }
}
