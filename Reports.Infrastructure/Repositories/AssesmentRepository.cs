using Microsoft.EntityFrameworkCore;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;
using Reports.Infrastructure.DbContexts;

namespace Reports.Infrastructure.Repositories;
internal class AssessmentRepository : IAssessmentRepository
{
    private readonly AppDbContext _dbContext;

    public AssessmentRepository(AppDbContext dbContext) =>
        _dbContext = dbContext;

    /// <inheritdoc />
    public void Add(Assessment assesment) =>
        _dbContext.Assessments.Add(assesment);

    /// <inheritdoc />
    public async Task<List<Assessment>> GetAllAsync() =>
        await _dbContext.Assessments.ToListAsync();

    public void Update(Assessment assessment) =>
        _dbContext.Assessments.Update(assessment);

    /// <inheritdoc />
    public async Task<Assessment?> GetByNameAsync(string name) =>
        await _dbContext.Assessments
            .FirstOrDefaultAsync(a => a.StudentId == 0);
    public async Task<Assessment?> GetByIdAsync(int id) =>
          await _dbContext.Assessments
              .FirstOrDefaultAsync(a => a.GoalId == id);
    public async Task<Assessment?> GetByStudentIdAsync(int id) =>
          await _dbContext.Assessments
              .FirstOrDefaultAsync(a => a.StudentId == id);
    public async Task<Assessment?> GetByStudentIdAndGoalId(int studentId, int goalId) => _dbContext.Assessments
    .FirstOrDefault(a => a.StudentId == studentId && a.GoalId == goalId);

}
