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
    public class FormationHistoryRepository : IFormationHistoryRepository
    {
        private readonly IMongoDatabase _database;

        public FormationHistoryRepository(IMongoDatabase database)
        {
            _database = database;
        }


        public async Task AddAsync(FormationHistory formationHistory)
            => await Collection.InsertOneAsync(formationHistory);

        public async Task<FormationHistory> GetAsync(Guid historyId)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == historyId);

        public async Task<IEnumerable<FormationHistory>> GetListAsync(Guid formationId)
            => await Collection
                .AsQueryable()
                .Where(x => x.Formation.Id == formationId)
                .ToListAsync();

        private IMongoCollection<FormationHistory> Collection
            => _database.GetCollection<FormationHistory>("FormationHistories");
    }
}
