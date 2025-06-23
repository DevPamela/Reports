using Reports.Domain.Entities;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Interfaces.Service;

namespace Reports.Service
{
    public class GoalService : IGoalService
    {
        private readonly IStudentService _studentService;
        private readonly IGoalRepository _goalRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAssessmentService _assessmentService;

        public GoalService(
            IStudentService studentService,
            IGoalRepository goalRepository,
            IUnitOfWork unitOfWork, IAssessmentService assessmentService)
        {
            _studentService = studentService;
            _goalRepository = goalRepository;
            _unitOfWork = unitOfWork;
            _assessmentService = assessmentService;
        }

        public async Task<List<Student>> SaveStudentsGoalsAsync(List<string> goals, int teacherId)
        {
            await SetAllPropertiesToFalse(teacherId);

            foreach (var g in goals)
            {
                var goal = await _goalRepository.GetByNameAsync(g, teacherId);

                SetIsSelectedAsTrue(goal);
            }
            await _unitOfWork.SaveChangesAsync();
            var goalsList = await _goalRepository.GetByIsSelectedProperty(teacherId);

            var students = await SetGoalsForStudentList(goalsList, teacherId);

            return students;
        }

        public async Task<List<Student>> SetGoalsForStudentList(List<Goal> goals, int teacherId)
        {
            var selectedStudents = await _studentService.GetByIsSelectedProperty(teacherId);


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
        public async Task SetStudentId(Student student, Goal goal, int teacherId)
        {
            var hasStudentId = await CheckIfHasStudentId(student, teacherId);
            if (!hasStudentId)
            {
                goal.StudentId = student.Id;
            }
        }
        public async Task<bool> CheckIfHasStudentId(Student student, int teacherId)
        {
            var goals = await _goalRepository.GetGoalsByTeacherIdAsync(teacherId);

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
                public async Task SetAllPropertiesToFalse(int teacherId)
        {
            SelectableEntity<Goal> selectableSomeEntity = new SelectableEntity<Goal>();
            var goals = await _goalRepository.GetGoalsByTeacherIdAsync(teacherId); 
            foreach (var goal in goals)
            {
                selectableSomeEntity.ClearSelection(goal);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddGoalsAsync(string collectionName, string goalName, string description, bool isGirlsGoal, int teacherId)
        {
            var existingGoal = await _goalRepository.GetGirlsGoalsByNameAndTeacherIdAsync(goalName, teacherId);

            if (existingGoal == null)
            {
                existingGoal = new GirlsGoal
                {
                    Name = goalName,
                    TeacherId = teacherId
                };

                _goalRepository.Add(existingGoal);
            }

            SetGoalDescription(existingGoal, collectionName, description, isGirlsGoal);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<string> GetDescriptionForParagrafAsync(Assessment assessment, int teacherId, bool isGirl)
        {
            var assessmentName = assessment.Goal.Name;
            var name = await _goalRepository.GetGirlsGoalsByNameAndTeacherIdAsync(assessment.Goal.Name, teacherId);
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            return assessmentName = assessment.IsSatisfactory switch
            {
                "S" => isGirl ? name.GirlsAtendedDescription : name.AtendedDescription,
                "PS" => isGirl ? name.GirlsParciallyAtendedDescription : name.ParciallyAtendedDescription,
                "I" => isGirl ? name.GirlsUnatendedDescription : name.UnatendedDescription,
                _ => string.Empty
            };
        }
                private void SetGoalDescription(Goal goal, string collectionName, string newDescription, bool isGirl)
        {

            var propertyMappings = new Dictionary<string, Action<Goal, string>>
    {
        { "Satisfatório", (g, desc) => g.AtendedDescription = desc },
        { "Parcialmente satisfatório", (g, desc) => g.ParciallyAtendedDescription = desc },
        { "Insatisfatório", (g, desc) => g.UnatendedDescription = desc }
    };
            if (!isGirl)
            {
                if (propertyMappings.TryGetValue(collectionName, out var setProperty))
                {
                    setProperty(goal, newDescription);
                }
            }
            else
            {
                if (goal is GirlsGoal girlsGoal)
                {
                    var girlsPropertyMappings = new Dictionary<string, Action<GirlsGoal, string>>
        {
            { "Satisfatório", (g, desc) => g.GirlsAtendedDescription = desc },
            { "Parcialmente satisfatório", (g, desc) => g.GirlsParciallyAtendedDescription = desc },
            { "Insatisfatório", (g, desc) => g.GirlsUnatendedDescription = desc }
        };

                    if (girlsPropertyMappings.TryGetValue(collectionName, out var setGirlsProperty))
                    {
                        setGirlsProperty(girlsGoal, newDescription);
                    }
                }
            }
        }

        /// <inheritdoc />
        public async Task<List<Goal>> GetGoalsListAsyncByTeacherId(int teacherId) =>
            await _goalRepository.GetGoalsByTeacherIdAsync(teacherId);

        /// <inheritdoc />
        public async Task<Goal> GetGoalByIdAsync(int id) =>
           await _goalRepository.GetByIdAsync(id);

        public async Task UpdateStudent(Goal goal, int teacherId)
        {
            try
            {
                var repo = await _goalRepository.GetByNameAsync(goal.Name, teacherId);
                repo.AtendedDescription = goal.AtendedDescription ?? repo.AtendedDescription;
                repo.UnatendedDescription = goal.UnatendedDescription ?? repo.UnatendedDescription;
                repo.ParciallyAtendedDescription = goal.ParciallyAtendedDescription ?? repo.ParciallyAtendedDescription;


                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Goal> GetByNameAsync(string goal, int teacherId) => await _goalRepository.GetByNameAsync(goal, teacherId);
    }
}