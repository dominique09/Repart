using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repart.Services.Formations.Domain.Models;

namespace Repart.Services.Formations.Domain.Repositories
{
    public interface IFormationRepository
    {
        Task AddAsync(Formation formation);
        Task<Formation> GetAsync(Guid formationId);
        Task<Formation> GetByNameAsync(string name);
        Task<Formation> GetByAbreviationAsync(string abreviation);
        Task<IEnumerable<Formation>> GetAllAsync();

        Task ModifyAsync(Formation formation);
    }
}
