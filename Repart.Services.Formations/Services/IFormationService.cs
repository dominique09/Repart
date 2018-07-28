using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repart.Common.Events.Formation;

namespace Repart.Services.Formations.Services
{
    public interface IFormationService
    {
        Task<IEnumerable<Formation>> GetAllAsync();
        Task<FormationWithHistory> GetWithHistoryAsync(Guid formationId);
        Task<Formation> GetAsync(Guid formationId);

        Task<Formation> AddAsync(string name, string abreviation, Guid createdBy);
        Task<FormationWithHistory> ModifyAsync(Formation formation);
    }
}
