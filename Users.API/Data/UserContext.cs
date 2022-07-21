using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.API.Entities;

namespace Users.API.Data
{
    public class UserContext : IUserContext
    {
        public IMongoCollection<User> Users { get; }

        public UserContext(IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetValue<string>("DBSettings:ConnectionString"));

            IMongoDatabase db = client.GetDatabase(configuration.GetValue<string>("DBSettings:DBName"));

            Users = db.GetCollection<User>(configuration.GetValue<string>("DBSettings:CollectionName"));

            //Populate Users in case the collection is empty - for testing
            UserContextPopulate.Populate(Users);
        }

    }
}
