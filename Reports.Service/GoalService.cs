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
        //private readonly IMongoClient _client;
        //private readonly string _databaseName;

        public GoalService(IStudentService studentService)
        {
            _studentService = studentService;
            //_client = new MongoClient(connectionString);
            //_databaseName = databaseName;
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

                var filter = Builders<Student>.Filter.Eq("Name", s.Name);
                var update = Builders<Student>.Update.Set("Goals", s.Goals);
                db.Student.UpdateOne(filter, update);
            }
            return students;
        }

        //public IMongoCollection<BsonDocument> GetCollection(string collectionName)
        //{
        //    //var database = _client.GetDatabase(_databaseName);
        //    //return database.GetCollection<BsonDocument>(collectionName);

        //}

        public void AddGoalInColletion(string collectionName, string goalName, string Description)
        {
            ContextMongoDb db = new ContextMongoDb();
            if (collectionName == "Satisfatório")
            {
                var goals = new GoalsSatisfatory
                {
                    Name = goalName,
                    Description = Description
                };

                db.GoalsSatisfatory.InsertOne(goals);
            }
            else if (collectionName == "Parcialmente satisfatório")
            {
                var goals = new GoalsPartiallySatisfactory
                {
                    Name = goalName,
                    Description = Description
                };
                db.GoalsPartiallySatisfactory.InsertOne(goals);
            }
            else if (collectionName == "Insatisfatório")
            {
                var goals = new GoalsUnsatisfactory
                {
                    Name = goalName,
                    Description = Description
                };
                db.GoalsUnsatisfactory.InsertOne(goals);
            }
        }

        public List<GoalsSatisfatory> GetGoalsSatisfactoryList()
        {
            ContextMongoDb db = new ContextMongoDb();
            var students = db.GoalsSatisfatory.Find(s => true).ToList();
            return students;
        }

        public List<GoalsUnsatisfactory> GetGoalsGoalsUnsatisfactoryList()
        {
            ContextMongoDb db = new ContextMongoDb();
            var students = db.GoalsUnsatisfactory.Find(s => true).ToList();
            return students;
        }

        public List<GoalsPartiallySatisfactory> GetGoalsPartiallySatisfactoryList()
        {
            ContextMongoDb db = new ContextMongoDb();
            var students = db.GoalsPartiallySatisfactory.Find(s => true).ToList();
            return students;
        }

        public void SaveSelectedGoals(Dictionary<string, Dictionary<string, string>> avaliacoes)
        {
            ContextMongoDb db = new ContextMongoDb();
            foreach (var aluno in avaliacoes)
            {
                var alunoSalvo = _studentService.GetStudentByName(aluno.Key);
                var students = _studentService.GetStudentsList();
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