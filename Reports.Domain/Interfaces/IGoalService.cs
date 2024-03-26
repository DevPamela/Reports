using MongoDB.Bson;
using MongoDB.Driver;
using Reports.Domain.Constants;
using Reports.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Domain.Interfaces
{
    public interface IGoalService
    {
        List<Student> SaveStudentsGoals(List<string> goals);
        void SaveSelectedGoals(Dictionary<string, Dictionary<string, string>> avaliacoes);
        public void AddGoalInColletion(string collectionName, string goalName, string Description);
        //void AddGoalForDataBase<T>(IMongoCollection<T> collection, T document);
        //IMongoCollection<BsonDocument> GetCollection(string collectionName);
        // Task AddGoalInColletion(string collectionName, GoalsSatisfatory goal);
    }
}
