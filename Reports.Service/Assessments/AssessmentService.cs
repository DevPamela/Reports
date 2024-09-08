using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Models;

namespace Reports.Service.Assessments
{
    public class AssessmentService : IAssessmentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IGoalRepository _goalRepository;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssessmentService(
            IStudentRepository studentRepository,
            IGoalRepository goalRepository,
            IUnitOfWork unitOfWork, IAssessmentRepository assessmentRepository)
        {
            _studentRepository = studentRepository;
            _goalRepository = goalRepository;
            _unitOfWork = unitOfWork;
            _assessmentRepository = assessmentRepository;
        }


        public async Task AddAssessmentForByGoalId(Goal goal, Student student)
        {
            var existingAssessment =await _assessmentRepository.GetByStudentIdAndGoalId(student.Id, goal.Id);
            
            if (existingAssessment == null)
            {
                var newAssessment = new Assessment
                {
                    StudentId = student.Id,
                    GoalId = goal.Id
                };
                _assessmentRepository.Add(newAssessment);

                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task AddIsAtendedInformation(List<Student> students)
        {
              await  ClearIsSatisfactoryValues();

            foreach (var student in students)
            {
                var studentName = await _studentRepository.GetByNameAsync(student.Name);

                if (studentName == null)
                {
                    return;
                }

                foreach (var assessment in student.Assessments)
                {
                    var assessmentToUpdate = await _assessmentRepository.GetByStudentIdAndGoalId(studentName.Id, assessment.GoalId);

                    if (assessmentToUpdate != null)
                    {
                        assessmentToUpdate.IsSatisfactory = assessment.IsSatisfactory;
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task ClearIsSatisfactoryValues()
        {
            var assessments = await _assessmentRepository.GetAllAsync();
            foreach (var assessment in assessments)
            {
                assessment.IsSatisfactory = "";
            }
             await _unitOfWork.SaveChangesAsync();
        }
        public async Task<List<Assessment>> GetAllAssessmentsAsync() => await _assessmentRepository.GetAllAsync();

        public async Task<List<Assessment?>> GetByStudentIdAsync(int id) => await _assessmentRepository.GetByStudentIdAsync(id);
       
        public async Task DeleteAllAsync(int studentId) => await _assessmentRepository.DeleteAllAsync(studentId);
    }
}