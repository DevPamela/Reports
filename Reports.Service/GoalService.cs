using Reports.Domain.Interfaces;
using Reports.Domain.Interfaces.Repositories;
using Reports.Domain.Models;

namespace Reports.Service
{
    public class GoalService : IGoalService
    {
        private readonly IStudentService _studentService;
        private readonly IStudentRepository _studentRepository;
        private readonly IGoalRepository _goalRepository;

        public GoalService(
            IStudentService studentService, 
            IStudentRepository studentRepository, 
            IGoalRepository goalRepository)
        {
            _studentService = studentService;
            _studentRepository = studentRepository;
            _goalRepository = goalRepository;
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

                await _studentRepository.UpdateOneStudentGoalsAsync(s.Name, s.Goals);
            }
            return students;
        }

        public async Task AddGoalInColletionAsync(string collectionName, string goalName, string description)
        {
            var name = await _goalRepository.GetByNameAsync(goalName);

            if (name == null)
            {
                var goals = new Goal { Name = goalName };
                await _goalRepository.AddOneAsync(goals);
                name = goals;
            }

            switch (collectionName)
            {
                case "Satisfatório":
                    await _goalRepository.UpdateOneGoalDescriptionAsync(
                        goalName, nameof(name.AtendedDescription), description);
                    break;
                case "Parcialmente satisfatório":
                    await _goalRepository.UpdateOneGoalDescriptionAsync(
                        goalName, nameof(name.ParciallyAtendedDescription), description);
                    break;
                case "Insatisfatório":
                    await _goalRepository.UpdateOneGoalDescriptionAsync(
                        goalName, nameof(name.UnatendedDescription), description);
                    break;
            }
        }


        public async Task<Student> GetDescriptionForParagrafAsync(Student student)
        {
            foreach (var g in student.Goals)
            {
                var name = await _goalRepository.GetByNameAsync(g.Name);

                if (name != null)
                {
                    switch (g.IsSatisfactory)
                    {
                        case "S":
                            g.Name = name.AtendedDescription;
                            break;
                        case "PS":
                            g.Name = name.ParciallyAtendedDescription;
                            break;
                        default:
                            g.Name = name.UnatendedDescription;
                            break;
                    }
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
                var alunoSalvo = await _studentService.GetStudentByNameAsync(aluno.Key);
          
                if (alunoSalvo != null)
                {
                    var goals = new List<Goal>();

                    foreach (var meta in aluno.Value)
                    {
                        var goal = new Goal
                        {
                            Name = meta.Key,
                            IsSatisfactory = meta.Value
                        };
                        goals.Add(goal);
                    }

                    await _studentRepository.UpdateOneStudentGoalsAsync(alunoSalvo.Name, goals);
                }
            }
        }
    }
}