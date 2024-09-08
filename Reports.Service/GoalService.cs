using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Interfaces.Service;
using Reports.Domain.Models;
using System.Text;


namespace Reports.Service
{
    public class GoalService : IGoalService
    {
        private readonly IStudentService _studentService;
        private readonly IGoalRepository _goalRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAssessmentService _assessmentService;
        private readonly IBulkInsertHelper _bulkInsertHelper;

        public GoalService(
            IStudentService studentService,
            IGoalRepository goalRepository,
            IUnitOfWork unitOfWork, IAssessmentService assessmentService, IBulkInsertHelper bulkInsertHelper)
        {
            _studentService = studentService;
            _goalRepository = goalRepository;
            _unitOfWork = unitOfWork;
            _assessmentService = assessmentService;
            _bulkInsertHelper = bulkInsertHelper;
        }

        public async Task<List<Student>> SaveStudentsGoalsAsync(List<string> goals)
        {
            await SetAllPropertiesToFalse();

            foreach (var g in goals)
            {
                var goal = await _goalRepository.GetByNameAsync(g);

                SetIsSelectedAsTrue(goal);
            }
            await _unitOfWork.SaveChangesAsync();
            var goalsList = await _goalRepository.GetByIsSelectedProperty();

            var students = await SetGoalsForStudentList(goalsList);

            return students;
        }

        public async Task<List<Student>> SetGoalsForStudentList(List<Goal> goals)
        {
            var selectedStudents = await _studentService.GetByIsSelectedProperty();
            

            foreach (var student in selectedStudents)
            {
                await _assessmentService.DeleteAllAsync(student.Id);

                foreach (var goal in goals)
                {
                    await _assessmentService.AddAssessmentForByGoalId(goal, student);
                }

                student.Goals = goals;
            }
            return selectedStudents.ToList();
        }
        public void SetIsSelectedAsTrue(Goal goal)
        {
            SelectableEntity<Goal> selectableSomeEntity = new SelectableEntity<Goal>();
            selectableSomeEntity.SetSelectionAsTrue(goal);
        }
        public async Task SetStudentId(Student student, Goal goal)
        {
            var hasStudentId = await CheckIfHasStudentId(student);
            if (!hasStudentId)
            {
                goal.StudentId = student.Id;
            }
        }
        public async Task<bool> CheckIfHasStudentId(Student student)
        {
            var goals = await _goalRepository.GetAllAsync();

            foreach (var goal in goals)
            {
                ;
                if (student.Id == goal.StudentId)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task SetAllPropertiesToFalse()
        {
            SelectableEntity<Goal> selectableSomeEntity = new SelectableEntity<Goal>();
            var goals = await _goalRepository.GetAllAsync();
            foreach (var goal in goals)
            {
                selectableSomeEntity.ClearSelection(goal);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddGoalInColletionAsync(string collectionName, string goalName, string description)
        {
            var goal = await _goalRepository.GetByNameAsync(goalName);
            var newDescription = await SetPronounForGender(description);
            if (goal == null)
            {
                goal = new Goal { Name = goalName };
                _goalRepository.Add(goal);
            }

            switch (collectionName)
            {
                case "Satisfatório":
                    goal.AtendedDescription = newDescription;
                    break;
                case "Parcialmente satisfatório":
                    goal.ParciallyAtendedDescription = newDescription;
                    break;
                case "Insatisfatório":
                    goal.UnatendedDescription = newDescription;
                    break;
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<string> GetDescriptionForParagrafAsync(Assessment assessment)
        {
            var assessmentName = assessment.Goal.Name;
            var name = await _goalRepository.GetByNameAsync(assessment.Goal.Name);
            if (name == null)
            {
                return assessmentName;
            }
            return assessmentName = assessment.IsSatisfactory switch
            {
                "S" => name.AtendedDescription,
                "PS" => name.ParciallyAtendedDescription,
                "I" => name.UnatendedDescription
            };
        }

        public async Task<List<Goal>> GetGoalsListAsync() =>
            await _goalRepository.GetAllAsync();

        public async Task<Goal> GetGoalByIdAsync(int id) =>
           await _goalRepository.GetByIdAsync(id);


        public async Task<string> SetPronounForGender(string description)
        {
            var adjustedDescription = new StringBuilder(description);

            adjustedDescription.Replace("O Aluno", "{0} alun{0}");

            adjustedDescription.Replace("Nome do aluno", "{1}");

            adjustedDescription.Replace("(o)", "{0}");

            return adjustedDescription.ToString();
        }
    }
}