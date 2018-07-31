using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repart.Common.Events.Intervenant;

namespace Repart.Services.Intervenants.Services
{
    public interface IIntervenantService
    {
        Task<Intervenant> AddAsync(string firstname, string lastname, Guid formationId);
        Task<Intervenant> ChangeFormationAsync(Guid intervenantId, Guid formationId);

        Task<IEnumerable<Intervenant>> GetAllAsync();
        Task<Intervenant> GetAsync(Guid intervenantId);
    }
}
