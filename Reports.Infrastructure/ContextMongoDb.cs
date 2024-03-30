using MongoDB.Driver;
using Reports.Domain.Constants;
using Reports.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Infrastructure
{
    public class ContextMongoDb
    {
        public ContextMongoDb()
        {
            try
            {
                MongoClientSettings setting = MongoClientSettings.
                    FromUrl(new MongoUrl(ConnectionString));
                if (IsSSL)
                {
                    setting.SslSettings = new SslSettings()
                    {
                        EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
                    };
                }
                var mongoClient = new MongoClient(setting);
                _database = mongoClient.GetDatabase(DatabaseName);
            }
            catch (Exception)
            {

                throw new Exception("Não foi possível conectar");
            }
        }
        public static string ConnectionString { get; set; }
        public static string DatabaseName { get; set; }
        public static bool IsSSL { get; set; }
        private IMongoDatabase _database { get; }

        //TODO adicionar Goals collection, pegar de lá, e adicionar lá o post de goals(addgoals)
        
        public IMongoCollection<Student> Student
        {
            get
            {
                return _database.GetCollection<Student>("Student");
            }
        }

        public IMongoCollection<Goal> Goals
        {
            get
            {
                return _database.GetCollection<Goal>("Goals");
            }
        }
    }
}