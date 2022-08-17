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
                    Id = "62d9f68de51a517ab820abab",
                    Name = "Maycon Freitas",
                    BirthDate = "1994-03-22",
                    Active = true
                },
                new User()
                {
                    Id = "62d9f68de51a517ab820abac",
                    Name = "Nikola Tesla",
                    BirthDate = "1865-07-10",
                    Active = false
                },
                new User()
                {
                    Id = "62d9f68de51a517ab820abad",
                    Name = "Thomas Edison",
                    BirthDate = "1847-02-11",
                    Active = false
                }
            };
        }
    }
}
