using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repart.Common.Events.Formation;
using Repart.Common.Exceptions;
using Repart.Services.Formations.Domain.Repositories;

namespace Repart.Services.Formations.Services
{
    public class FormationService : IFormationService
    {
        private readonly IFormationRepository _formationRepository;
        private readonly IFormationHistoryRepository _formationHistoryRepository;

        public FormationService(IFormationRepository formationRepository,
            IFormationHistoryRepository formationHistoryRepository)
        {
            _formationHistoryRepository = formationHistoryRepository;
            _formationRepository = formationRepository;
        }

        public async Task<IEnumerable<Formation>> GetAllAsync()
            => (await _formationRepository.GetAllAsync()).Select(MapFormation);

        public async Task<FormationWithHistory> GetWithHistoryAsync(Guid formationId)
            => await MapFormationWithHistory(await FindFormation(formationId));

        public async Task<Formation> GetAsync(Guid formationId)
            => MapFormation(await FindFormation(formationId));

        public async Task<Formation> AddAsync(string name, string abreviation, Guid createdBy)
        {
            if(_formationRepository.GetByNameAsync(name).Result != null
                || _formationRepository.GetByAbreviationAsync(abreviation).Result != null)
                throw new RepartException("formation_already_exist",
                    $"Une formation semble déjà exister ({name}, {abreviation.ToUpperInvariant()})");

            var formation = new Domain.Models.Formation(name, abreviation);
            var formationHistory = new Domain.Models.FormationHistory(createdBy, formation);

            await _formationRepository.AddAsync(formation);
            await _formationHistoryRepository.AddAsync(formationHistory);

            return MapFormation(formation);
        }

        public Task<FormationWithHistory> ModifyAsync(Formation formation)
        {
            throw new NotImplementedException();
        }

        private async Task<Domain.Models.Formation> FindFormation(Guid formationId)
        {
            var formation = await _formationRepository.GetAsync(formationId);
            if(formation == null)
                throw new RepartException("formation_not_found",
                    $"Formation non trouvée. ({formationId})");

            return formation;
        }

        private static Formation MapFormation(Domain.Models.Formation formation)
            => new Formation(formation.Id, formation.Name, formation.Abreviation);

        private static FormationHistory MapFormationHistory(Domain.Models.FormationHistory history)
            => new FormationHistory(history.Id, history.UpdateAt, history.UpdateBy, MapFormation(history.Formation));

        private async Task<FormationWithHistory> MapFormationWithHistory(Domain.Models.Formation formation)
            => new FormationWithHistory(formation.Id, formation.Name, formation.Abreviation, 
                (await _formationHistoryRepository.GetListAsync(formation.Id)).Select(MapFormationHistory));
    }
}
