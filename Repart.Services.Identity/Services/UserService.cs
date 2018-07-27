using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repart.Common.Auth;
using Repart.Common.Events;
using Repart.Common.Events.Identity;
using Repart.Common.Exceptions;
using Repart.Services.Identity.Domain.Models;
using Repart.Services.Identity.Domain.Repositories;
using Repart.Services.Identity.Domain.Services;
using User = Repart.Common.Events.Identity.User;

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
            IJwtHandler _jwtHandler)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _encrypter = encrypter;
            this._jwtHandler = _jwtHandler;
        }

        public async Task<User> RegisterAsync(string email, string password, string name, IEnumerable<Guid> roles)
        {
            var user = await _userRepository.GetAsync(email);
            if(user != null)
                throw new RepartException("user_already_exist",
                    $"Un usager avec cette adresse courriel existe déjà ! ({email})");

            foreach (var role in roles)
            {
                if(_roleRepository.GetAsync(role) == null)
                    throw new RepartException("role_not_exist",
                        $"Le role ({role}) n'existe pas.");
            }

            user = new Domain.Models.User(email, name, roles);
            user.SetPassword(password, _encrypter);
            await _userRepository.AddAsync(user);

            return new User(user.Email, user.Name, user.Roles, user.CreatedAt);
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

        public async Task<User> AddToRole(Guid userId, Guid roleId)
        {
            var user = await _userRepository.GetAsync(userId);
            if(user == null)
                throw new RepartException("user_not_found",
                    $"Usager non trouvé. ({userId})");

            var role = await _roleRepository.GetAsync(roleId);
            if(role == null)
                throw new RepartException("role_not_found",
                    $"Role non trouvé. ({roleId})");

            await _userRepository.ModifyAsync(user.AddRole(roleId));

            return new User(user.Email, user.Name, user.Roles, user.CreatedAt);
        }

        public async Task<User> RemoveFromRole(Guid userId, Guid roleId)
        {
            var user = await _userRepository.GetAsync(userId);
            if(user == null)
                throw new RepartException("user_not_found",
                    $"Usager non trouvé. ({userId})");

            var role = await _roleRepository.GetAsync(roleId);
            if(role == null)
                throw new RepartException("role_not_found",
                    $"Role non trouvé. ({roleId})");

            await _userRepository.ModifyAsync(user.RemoveRole(roleId));

            return new User(user.Email, user.Name, user.Roles, user.CreatedAt);
        }

        public async Task<User> GetAsync(Guid userId)
        {
            var user = await _userRepository.GetAsync(userId);
            if(user == null)
                throw new RepartException("user_not_found",
                    $"Usager non trouvé. ({userId})");

            return new User(user.Email, user.Name, user.Roles, user.CreatedAt);
        }
    }
}
