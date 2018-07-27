using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Repart.Services.Identity.Domain.Models;
using Repart.Services.Identity.Domain.Repositories;

namespace Repart.Services.Identity.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IMongoDatabase _database;

        public RoleRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Role> GetAsync(Guid id)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<Role> GetAsync(string name)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(r => r.Name == name);

        public async Task<IEnumerable<Role>> GetAll()
            => await Collection
                .AsQueryable()
                .ToListAsync();

        public async Task AddAsync(Role role)
            => await Collection
                .InsertOneAsync(role);

        private IMongoCollection<Role> Collection
            => _database.GetCollection<Role>("Roles");
    }
}
