using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Repart.Services.Formations.Domain.Models;
using Repart.Services.Formations.Domain.Repositories;

namespace Repart.Services.Formations.Repositories
{
    public class FormationRepository : IFormationRepository
    {
        private readonly IMongoDatabase _database;

        public FormationRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(Formation formation)
            => await Collection.InsertOneAsync(formation);

        public async Task<Formation> GetAsync(Guid formationId)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == formationId);

        public async Task<Formation> GetByNameAsync(string name)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Name == name);

        public async Task<Formation> GetByAbreviationAsync(string abreviation)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Abreviation == abreviation.ToUpperInvariant());

        public async Task<IEnumerable<Formation>> GetAllAsync()
            => await Collection
                .AsQueryable()
                .ToListAsync();

        public async Task ModifyAsync(Formation formation)
            => await Collection
                .ReplaceOneAsync(x => x.Id == formation.Id, formation);

        private IMongoCollection<Formation> Collection
            => _database.GetCollection<Formation>("Formations");
    }
}
