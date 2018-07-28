using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repart.Common.Auth;
using Repart.Common.Exceptions;
using Repart.Services.Identity.Domain.Repositories;
using Repart.Services.Identity.Domain.Services;
using Role = Repart.Services.Identity.Domain.Models.Role;
using RoleEvent = Repart.Common.Events.Identity.Role;
using User = Repart.Services.Identity.Domain.Models.User;
using UserEvent = Repart.Common.Events.Identity.User;

namespace Repart.Services.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEncrypter _encrypter;
        private readonly IJwtHandler _jwtHandler;

        public UserService(IUserRepository userRepository, 
            IRoleRepository roleRepository,
            IEncrypter encrypter,
            IJwtHandler jwtHandler)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _encrypter = encrypter;
            _jwtHandler = jwtHandler;
        }

        public async Task<UserEvent> RegisterAsync(string email, string password, string name, IEnumerable<Guid> roles)
        {
            var user = await _userRepository.GetAsync(email);
            if(user != null)
                throw new RepartException("user_already_exist",
                    $"Un usager avec cette adresse courriel existe déjà ! ({email})");

            foreach (var role in roles)
            {
                await FindRole(role);
            }

            user = new User(email, name, roles);
            user.SetPassword(password, _encrypter);
            await _userRepository.AddAsync(user);
            return MapUser(user);
        }

        public async Task<JsonWebToken> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if(user == null)
                throw new RepartException("invalid_credentials",
                    $"Les informations de connexions sont invalides.");

            if(!user.ValidatePassword(password, _encrypter))
                throw new RepartException("invalid_credentials",
                    $"Les informations de connexions sont invalides.");
            
            var roles = user.Roles.Select(x => _roleRepository.GetAsync(x).Result.Name);
            
            return _jwtHandler.Create(user.Id, roles);
        }

        public async Task<UserEvent> AddToRole(Guid userId, Guid roleId)
        {
            var user = await FindUser(userId);
            await FindRole(roleId);

            await _userRepository.ModifyAsync(user.AddRole(roleId));
            return MapUser(user);
        }

        public async Task<UserEvent> RemoveFromRole(Guid userId, Guid roleId)
        {
            var user = await FindUser(userId);
            var role = await FindRole(roleId);

            await _userRepository.ModifyAsync(user.RemoveRole(roleId));
            return MapUser(user);
        }

        public async Task<UserEvent> GetAsync(Guid userId)
        {
            var user = await FindUser(userId);
            return MapUser(user);
        }

        public async Task<IEnumerable<UserEvent>> GetAll()
            => (await _userRepository.GetAll()).Select(MapUser);
        
        public async Task<UserEvent> ToggleActive(Guid userId)
        {
            var user = await FindUser(userId);
            user.ToggleActive();
            await _userRepository.ModifyAsync(user);

            return MapUser(user);
        }

        private IEnumerable<RoleEvent> GetRoles(IEnumerable<Guid> roleIds)
            => roleIds
                .Select(roleId => _roleRepository.GetAsync(roleId).Result)
                .Select(r => new RoleEvent(r.Id, r.Name));

        private async Task<User> FindUser(Guid userId)
        {
            var user = await _userRepository.GetAsync(userId);
            if(user == null)
                throw new RepartException("user_not_found",
                    $"Usager non trouvé. ({userId})");

            return user;
        }

        private UserEvent MapUser(User user)
            => new UserEvent(user.Id, user.Email, user.Name, user.Active, GetRoles(user.Roles), user.CreatedAt);
        
        private async Task<Role> FindRole(Guid roleId)
        {
            var role = await _roleRepository.GetAsync(roleId);
            if(role == null)
                throw new RepartException("role_not_found",
                    $"Role non trouvé. ({roleId})");
            
            return role;
        }

    }
}
