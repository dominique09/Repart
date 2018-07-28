using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repart.Services.Formations.Domain.Models;

namespace Repart.Services.Formations.Domain.Repositories
{
    public interface IFormationHistoryRepository
    {
        Task AddAsync(FormationHistory formationHistory);
        Task<FormationHistory> GetAsync(Guid historyId);

        Task<IEnumerable<FormationHistory>> GetListAsync(Guid formationId);
    }
}
