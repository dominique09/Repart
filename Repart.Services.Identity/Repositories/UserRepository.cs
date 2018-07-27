using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Repart.Services.Identity.Domain.Models;
using Repart.Services.Identity.Domain.Repositories;

namespace Repart.Services.Identity.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _database;

        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<User> GetAsync(Guid id)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<User> GetAsync(string email)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Email == email.ToLowerInvariant());

        public async Task AddAsync(User user)
            => await Collection
                .InsertOneAsync(user);

        public async Task<IEnumerable<User>> GetAll()
            => await Collection
                .AsQueryable()
                .ToListAsync();

        public async Task ModifyAsync(User user)
            => await Collection
                .ReplaceOneAsync(x => x.Id == user.Id, user);

        private IMongoCollection<User> Collection
            => _database.GetCollection<User>("Users");
    }
}
