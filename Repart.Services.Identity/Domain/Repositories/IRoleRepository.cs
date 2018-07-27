using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repart.Services.Identity.Domain.Models;

namespace Repart.Services.Identity.Domain.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> GetAsync(Guid id);
        Task<Role> GetAsync(string name);
        Task<IEnumerable<Role>> GetAll();

        Task AddAsync(Role role);
    }
}
