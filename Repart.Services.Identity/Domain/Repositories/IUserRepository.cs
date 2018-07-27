using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repart.Services.Identity.Domain.Models;

namespace Repart.Services.Identity.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Guid id);
        Task<User> GetAsync(string email);

        Task AddAsync(User user);
        Task<IEnumerable<User>> GetAll();

        Task ModifyAsync(User user);
    }
}
