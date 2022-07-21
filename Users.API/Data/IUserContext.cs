using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.API.Entities;

namespace Users.API.Data
{
    public interface IUserContext
    {
        IMongoCollection<User> Users { get; }
    }
}
