using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.API.Entities;

namespace Users.API.Data
{
    public class UserContextPopulate
    {
        public static void Populate(IMongoCollection<User> users)
        {
            bool userExists = users.Find(x => true).Any();
            if (!userExists) users.InsertMany(CreateTestUsers());
        }

        private static IEnumerable<User> CreateTestUsers()
        {
            return new List<User>()
            {
                new User()
                {
                    Name = "Maycon Freitas",
                    BirthDate = "1994-03-22",
                    Active = true
                },
                new User()
                {
                    Name = "Cristian LaFuente",
                    BirthDate = "1983-03-21",
                    Active = true
                },
                new User()
                {
                    Name = "Gustavo Gusmao",
                    BirthDate = "1995-09-05",
                    Active = true
                }
            };
        }
    }
}
