using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Reports.Application.Settings;
using Reports.Domain.Models;

namespace Reports.Infrastructure
{
    internal class MongoDbContext
    {
        public IMongoCollection<Student> StudentsCollection { get; }
        public IMongoCollection<Goal> GoalsCollection { get; }

        public MongoDbContext(IOptions<ReportsDatabaseSettings> options)
        {
            var reportsDatabaseSettings = options.Value;

            var mongoSettings = MongoClientSettings.FromUrl(
                new MongoUrl(reportsDatabaseSettings.ConnectionString));
            
            if (reportsDatabaseSettings.IsSSL)
            {
                mongoSettings.SslSettings = new SslSettings()
                {
                    EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
                };
            }

            var mongoClient = new MongoClient(mongoSettings);

            var mongoDatabase = mongoClient.GetDatabase(
                reportsDatabaseSettings.DatabaseName);

            StudentsCollection = mongoDatabase.GetCollection<Student>(
                reportsDatabaseSettings.StudentsCollectionName);

            GoalsCollection = mongoDatabase.GetCollection<Goal>(
                reportsDatabaseSettings.GoalsCollectionName);
        }

        //TODO adicionar Goals collection, pegar de lá, e adicionar lá o post de goals(addgoals)
    }
}