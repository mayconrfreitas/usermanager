using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.API.Data;
using Users.API.Entities;

namespace Users.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private IUserContext _context;
        public UserRepository(IUserContext context)
        {
            _context = context;
        }

        public async Task CreateUser(User user)
        {
            await _context.Users.InsertOneAsync(user);
        }

        public async Task<bool> DeleteUser(string id)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(x => x.Id, id);

            DeleteResult deleteResult = await _context.Users.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        //public async Task<bool> DeleteUsers(IEnumerable<string> ids)
        //{
        //    FilterDefinition<User> filter = Builders<User>.Filter.In(x => x.Id, ids);

        //    DeleteResult deleteResult = await _context.Users.DeleteManyAsync(filter);

        //    return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        //}

        public async Task<IEnumerable<User>> GetActiveUsers()
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(x => x.Active, true);

            return await _context.Users.Find(filter).ToListAsync();
        }

        public async Task<User> GetUser(string id)
        {
            return await _context.Users.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.Find(x => true).ToListAsync();
        }

        public async Task<bool> UpdateUserState(string id)
        {
            User user = await _context.Users.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (user is not null)
            {
                user.Active = !user.Active;
                ReplaceOneResult replaceResult = await _context.Users.ReplaceOneAsync(filter: x => x.Id == user.Id, replacement: user);

                return replaceResult.IsAcknowledged && replaceResult.ModifiedCount > 0;
            }

            return false;
            
        }
    }
}
