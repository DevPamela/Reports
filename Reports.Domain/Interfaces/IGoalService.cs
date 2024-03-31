using Reports.Domain.Models;

namespace Reports.Domain.Interfaces
{
    public interface IGoalService
    {
        Task<List<Student>> SaveStudentsGoalsAsync(List<string> goals);
        Task AddGoalInColletionAsync(string collectionName, string goalName, string description);
        Task<Student> GetDescriptionForParagrafAsync(Student student);
        Task<List<Goal>> GetGoalsListAsync();
        Task SaveSelectedGoalsAsync(Dictionary<string, Dictionary<string, string>> avaliacoes);
    }
}
