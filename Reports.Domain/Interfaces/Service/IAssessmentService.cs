using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Domain.Interfaces.Service
{
    public interface IAssessmentService
    {
        /// <summary>
        /// Add a new assessment.
        /// </summary>
        /// <param name="goal"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        Task AddAssessmentForByGoalId(Goal goal, Student student);

        /// <summary>
        /// Add Satisfactory description.
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        Task AddIsAtendedInformation(List<Student> student);

        /// <summary>
        /// Get Assessments list.
        /// </summary>
        /// <returns></returns>
        Task<List<Assessment>> GetAllAssessmentsAsync();

        /// <summary>
        /// Get the assessment by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Assessment> GetById(int id);
    }
}
