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
        Task AddGoalsAsync(string collectionName, string goalName, string description, bool isGirlsGoal, int teacherId);



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
        /// <summary>
        /// Get a Goal by Teacher Id.
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        Task<List<Goal>> GetGoalsListAsyncByTeacherId(int teacherId);

        /// <summary>
        /// Get a description to be written in the report.
        /// </summary>
        /// <param name="assessment"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        Task<string> GetDescriptionForParagrafAsync(Assessment assessment, int teacherId, bool isAGirl);
    }
}