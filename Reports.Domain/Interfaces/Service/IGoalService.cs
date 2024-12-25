using Reports.Domain.Entities;

namespace Reports.Domain.Interfaces.Service
{
    public interface IGoalService
    {
        /// <summary>
        /// Save the selected goals foreach student.
        /// </summary>
        /// <param name="goals"></param>
        /// <returns></returns>
        Task<List<Student>> SaveStudentsGoalsAsync(List<string> goals);

        /// <summary>
        /// Save a new <see cref="Goal">.
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="goalName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task AddGoalInColletionAsync(string collectionName, string goalName, string description);

        /// <summary>
        /// Get the paragraph text using the IsSatisfactory property.
        /// </summary>
        /// <param name="assessment"></param>
        /// <returns></returns>
        Task<string> GetDescriptionForParagrafAsync(Assessment assessment);

        /// <summary>
        /// Get the <see cref="Goal"/> list.
        /// </summary>
        /// <returns></returns>
        Task<List<Goal>> GetGoalsListAsync();

        /// <summary>
        /// Get a specific <see cref="Goal"/>by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Goal> GetGoalByIdAsync(int id);
    }
}
