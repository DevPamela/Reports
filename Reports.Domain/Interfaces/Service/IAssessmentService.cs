using Reports.Domain.Entities;

namespace Reports.Domain.Interfaces.Service
{
    public interface IAssessmentService
    {
        /// <summary>
        /// Add a new <see cref="Assessment">.
        /// </summary>
        /// <param name="goal">The selected <see cref="Goal"/></param>
        /// <param name="student">The selected <see cref="Student"/></param>
        Task AddAssessmentForByGoalId(Goal goal, Student student);

        /// <summary>
        /// Add Satisfactory description.
        /// </summary>
        /// <param name="student"></param>
        Task AddIsAtendedInformation(List<Student> student, int teacherId);

        /// <summary>
        /// Get the <see cref="Assessment"> by <see cref="Student"/> Id .
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A list of <see cref="Assessment"/></returns>
        Task<List<Assessment?>> GetByStudentIdAsync(int id);

        /// <summary>
        /// Delete all <see cref="Assessment"/>for the <see cref="Student"/>id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        Task DeleteAllAsync(int studentId);
    }
}