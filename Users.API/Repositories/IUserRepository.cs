using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.API.Entities;

namespace Users.API.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<IEnumerable<User>> GetActiveUsers();
        Task<User> GetUser(string id);

        Task CreateUser(User user);
        Task<bool> UpdateUserState(string id);
        Task<bool> DeleteUser(string id);
    }
}
