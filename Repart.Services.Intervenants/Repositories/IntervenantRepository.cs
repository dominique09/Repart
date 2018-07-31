using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Repart.Services.Intervenants.Domain.Models;
using Repart.Services.Intervenants.Domain.Repositories;

namespace Repart.Services.Intervenants.Repositories
{
    public class IntervenantRepository : IIntervenantRepository
    {
        private readonly IMongoDatabase _database;

        public IntervenantRepository(IMongoDatabase database)
        {
            _database = database;
        }

        private IMongoCollection<Intervenant> Collection
            => _database.GetCollection<Intervenant>("Intervenants");

        public async Task AddAsync(Intervenant intervenant)
            => await Collection
                .InsertOneAsync(intervenant);

        public async Task<Intervenant> GetAsync(Guid intervenantId)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(i => i.Id == intervenantId);

        public async Task<Intervenant> GetByInitiales(string initiales)
            => await Collection
                .AsQueryable()
                .FirstOrDefaultAsync(i => i.Initiales == initiales);

        public async Task<IEnumerable<Intervenant>> GetAllAsync()
            => await Collection
                .AsQueryable()
                .ToListAsync();

        public async Task ModifyAsync(Intervenant intervenant)
            => await Collection
                .ReplaceOneAsync(x => x.Id == intervenant.Id, intervenant);
    }
}
