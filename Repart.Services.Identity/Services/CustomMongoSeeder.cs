using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Repart.Common.Mongo;
using Repart.Services.Identity.Domain.Models;
using Repart.Services.Identity.Domain.Repositories;

namespace Repart.Services.Identity.Services
{
    public class CustomMongoSeeder : MongoSeeder
    {
        private readonly IRoleRepository _roleRepository;

        public CustomMongoSeeder(IMongoDatabase database,
            IRoleRepository roleRepository) : base(database)
        {
            _roleRepository = roleRepository;
        }

        protected override async Task CustomSeedAsync()
        {
            var roles = new List<string>
            {
                "SUPER_ADMIN",
                "GESTION_UTILISATEUR", "CONSULTATION_UTILISATEUR",
                "GESTION_INTERVENANT", "CONSULTATION_INTERVENANT",
                "GESTION_FORMATION", "CONSULTATION_FORMATION",
            };

            await Task.WhenAll(roles.Select(r => _roleRepository.AddAsync(new Role(r))));
        }
        
    }
}
