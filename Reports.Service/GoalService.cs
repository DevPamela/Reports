using DocumentFormat.OpenXml.Wordprocessing;
using MathNet.Numerics.Distributions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Reports.Domain.Constants;
using Reports.Domain.Interfaces;
using Reports.Domain.Models;
using Reports.Infrastructure;

namespace Reports.Service
{
    public class GoalService : IGoalService
    {
        private readonly IStudentService _studentService;

        public GoalService(IStudentService studentService)
        {
            _studentService = studentService;
        }
        public List<Student> SaveStudentsGoals(List<string> goals)
        {
            ContextMongoDb db = new ContextMongoDb();
            var students = db.Student.Find(s => true).ToList();            

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

                var filter2 = Builders<Student>.Filter.Eq("Name", s.Name);
                var update3 = Builders<Student>.Update.Set("Goals", s.Goals);
                db.Student.UpdateOne(filter2, update3);
            }
            return students;
        }

        public void AddGoalInColletion(string collectionName, string goalName, string Description)
        {
            ContextMongoDb db = new ContextMongoDb();
            var name = db.Goals.Find(s => s.Name == goalName).FirstOrDefault();
            var update = Builders<Goal>.Update;

            if (name == null)
            {
                var goals = new Goal { Name = goalName };
                db.Goals.InsertOne(goals);
                name = goals;
            }

            switch (collectionName)
            {
                case "Satisfatório":
                    db.Goals.UpdateOne(s => s.Name == goalName, update.Set(s => s.AtendedDescription, Description));
                    break;
                case "Parcialmente satisfatório":
                    db.Goals.UpdateOne(s => s.Name == goalName, update.Set(s => s.ParciallyAtendedDescription, Description));
                    break;
                case "Insatisfatório":
                    db.Goals.UpdateOne(s => s.Name == goalName, update.Set(s => s.UnatendedDescription, Description));
                    break;
            }
        }


        public Student GetDescriptionForParagraf(Student student)
        {
            ContextMongoDb db = new ContextMongoDb();

            foreach (var g in student.Goals)
            {
                var name = db.Goals.Find(s => s.Name == g.Name).FirstOrDefault();

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

        public List<Goal> GetGoalsList()
        {
            ContextMongoDb db = new ContextMongoDb();
            var goals = db.Goals.Find(g => true).ToList();
            return goals;
        }

        public void SaveSelectedGoals(Dictionary<string, Dictionary<string, string>> avaliacoes)
        {
            ContextMongoDb db = new ContextMongoDb();
            foreach (var aluno in avaliacoes)
            {
                var alunoSalvo = _studentService.GetStudentByName(aluno.Key);
          
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
                    var filter = Builders<Student>.Filter.Empty;
                    var update = Builders<Student>.Update.Set("Goals", goals);

                    db.Student.UpdateManyAsync(filter, update);
                }
            }
        }
    }
}