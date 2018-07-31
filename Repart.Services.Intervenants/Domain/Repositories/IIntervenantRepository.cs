using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repart.Services.Intervenants.Domain.Models;

namespace Repart.Services.Intervenants.Domain.Repositories
{
    public interface IIntervenantRepository
    {
        Task AddAsync(Intervenant intervenant);
        Task<Intervenant> GetAsync(Guid intervenantId);
        Task<Intervenant> GetByInitiales(string initiales);
        Task<IEnumerable<Intervenant>> GetAllAsync();

        Task ModifyAsync(Intervenant intervenant);
    }
}
