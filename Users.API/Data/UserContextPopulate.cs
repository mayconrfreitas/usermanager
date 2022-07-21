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
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "Maycon Freitas",
                    BirthDate = "1994-03-22",
                    Active = true
                },
                new User()
                {
                    Id = "602d2149e773f2a3990b47f6",
                    Name = "Cristian LaFuente",
                    BirthDate = "1983-03-21",
                    Active = true
                },
                new User()
                {
                    Id = "602d2149e773f2a3990b47f7",
                    Name = "Gustavo Gusmao",
                    BirthDate = "1995-09-05",
                    Active = true
                }
            };
        }
    }
}
