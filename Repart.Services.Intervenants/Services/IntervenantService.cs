using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repart.Common.Events.Intervenant;
using Repart.Common.Exceptions;
using Repart.Services.Intervenants.Domain.Repositories;

namespace Repart.Services.Intervenants.Services
{
    public class IntervenantService : IIntervenantService
    {
        private readonly IIntervenantRepository _intervenantRepository;

        public IntervenantService(IIntervenantRepository intervenantRepository)
        {
            _intervenantRepository = intervenantRepository;
        }

        public async Task<Intervenant> AddAsync(string firstname, string lastname, Guid formationId)
        {
            var intervenant = new Domain.Models.Intervenant(firstname, 
                lastname, 
                await CreateInitialesUnique(firstname, lastname), 
                formationId);

            return MapIntervenant(intervenant);
        }


        public async Task<Intervenant> ChangeFormationAsync(Guid intervenantId, Guid formationId)
        {
            var intervenant = await FindIntervenant(intervenantId);
            if(intervenant.FormationId == formationId)
                throw new RepartException("intervenant_already_have_formation",
                    $"L'intervenant ({intervenantId} possède déjà cette formation ({formationId}).)");

            intervenant.ChangeFormation(formationId);

            await _intervenantRepository.ModifyAsync(intervenant);
            return MapIntervenant(intervenant);
        }

        public async Task<IEnumerable<Intervenant>> GetAllAsync()
            => (await _intervenantRepository.GetAllAsync()).Select(MapIntervenant);


        public async Task<Intervenant> GetAsync(Guid intervenantId)
            => MapIntervenant(await _intervenantRepository.GetAsync(intervenantId));
        
        private static Intervenant MapIntervenant(Domain.Models.Intervenant intervenant)
            => new Intervenant(intervenant.Id,
                intervenant.Firstname, intervenant.Lastname,
                intervenant.Initiales, intervenant.FormationId, intervenant.CreatedAt);
        
        private async Task<string> CreateInitialesUnique(string firstname, string lastname)
        {
            var initiales = "";
            var iFirstname = 1;
            var iLastname = 1;
            var firstnameNext = false;
            var firstChar = "";
            var secondChar = "";
            var lastChanceIncrement = 1;

            do
            {
                if(iFirstname <= firstname.Length)
                    firstChar = firstname.ToUpperInvariant().Substring(iFirstname - 1, iFirstname);

                if(iFirstname <= firstname.Length)
                    secondChar = lastname.ToUpperInvariant().Substring(iLastname - 1, iLastname);

                var lastInitiales = initiales;
                initiales = string.Concat(firstChar, secondChar);

                if (lastInitiales == initiales)
                    initiales = string.Concat(initiales.Substring(0,2), lastChanceIncrement++);

                if (firstnameNext)
                    iFirstname++;
                else
                    iLastname++;

                firstnameNext = !firstnameNext;

            } while (await _intervenantRepository.GetByInitiales(initiales) != null);

            return initiales;
        }

        private async Task<Domain.Models.Intervenant> FindIntervenant(Guid intervenantId)
        {
            var intervenant = await _intervenantRepository.GetAsync(intervenantId);
            if(intervenant == null)
                throw new RepartException("intervenant_not_found",
                    $"Intervenant non trouvée. ({intervenantId})");

            return intervenant;
        }
    }
}
