using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repart.Common.Auth;
using Repart.Common.Events.Identity;

namespace Repart.Services.Identity.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(string email, string password, string name, IEnumerable<Guid> roles);
        Task<JsonWebToken> LoginAsync(string email, string password);

        Task<User> AddToRole(Guid userId, Guid roleId);
        Task<User> RemoveFromRole(Guid userId, Guid roleId);
        Task<User> GetAsync(Guid userId);
        Task<IEnumerable<User>> GetAll();

        Task<User> ToggleActive(Guid userId);
        
    }
}
