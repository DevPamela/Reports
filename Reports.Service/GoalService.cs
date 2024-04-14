using Reports.Domain.Interfaces;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;

namespace Reports.Service
{
    public class GoalService : IGoalService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IGoalRepository _goalRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GoalService(
            IStudentRepository studentRepository,
            IGoalRepository goalRepository,
            IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository;
            _goalRepository = goalRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Student>> SaveStudentsGoalsAsync(List<string> goals)
        {
            var students = await _studentRepository.GetAllAsync();

            foreach (var s in students)
            {
                var goalList = new List<Goal>();

                foreach (var g in goals)
                {
                    var goal = new Goal
                    {
                        Name = g
                    };
                    goalList.Add(goal);
                }
                s.Goals.AddRange(goalList);

                await _unitOfWork.SaveChangesAsync();
            }
            return students;
        }

        public async Task AddGoalInColletionAsync(string collectionName, string goalName, string description)
        {
            var goal = await _goalRepository.GetByNameAsync(goalName);

            if (goal == null)
            {
                goal = new Goal { Name = goalName };
                _goalRepository.Add(goal);
            }

            switch (collectionName)
            {
                case "Satisfatório":
                    goal.AtendedDescription = description;
                    break;
                case "Parcialmente satisfatório":
                    goal.ParciallyAtendedDescription = description;
                    break;
                case "Insatisfatório":
                    goal.UnatendedDescription = description;
                    break;
            }

            await _unitOfWork.SaveChangesAsync();
        }


        public async Task<Student> GetDescriptionForParagrafAsync(Student student)
        {
            foreach (var g in student.Goals)
            {
                var name = await _goalRepository.GetByNameAsync(g.Name);

                if (name != null)
                {
                    //TODO: CHECK LATER
                    //switch (g.IsSatisfactory)
                    //{
                    //    case "S":
                    //        g.Name = name.AtendedDescription;
                    //        break;
                    //    case "PS":
                    //        g.Name = name.ParciallyAtendedDescription;
                    //        break;
                    //    default:
                    //        g.Name = name.UnatendedDescription;
                    //        break;
                    //}
                }
            }
            return student;
        }

        public async Task<List<Goal>> GetGoalsListAsync() =>
            await _goalRepository.GetAllAsync();

        public async Task SaveSelectedGoalsAsync(Dictionary<string, Dictionary<string, string>> avaliacoes)
        {
            foreach (var aluno in avaliacoes)
            {
                var alunoSalvo = await _studentRepository.GetByNameAsync(aluno.Key);

                if (alunoSalvo != null)
                {
                    var goals = new List<Goal>();

                    foreach (var meta in aluno.Value)
                    {
                        var goal = new Goal
                        {
                            Name = meta.Key,
                            //TODO: CHECK LATER
                            //IsSatisfactory = meta.Value
                        };
                        goals.Add(goal);
                    }

                    await _unitOfWork.SaveChangesAsync();
                }
            }
        }
    }
}