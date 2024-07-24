using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;

namespace Reports.Domain.Interfaces.Service
{
    public interface IGoalService
    {
        Task<List<Student>> SaveStudentsGoalsAsync(List<string> goals);
        Task AddGoalInColletionAsync(string collectionName, string goalName, string description);
        Task<string> GetDescriptionForParagrafAsync(Assessment assessment);
        Task<List<Goal>> GetGoalsListAsync();
        Task<Goal> GetGoalByIdAsync(int id);
    }
}
