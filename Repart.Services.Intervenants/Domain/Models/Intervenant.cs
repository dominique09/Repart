using System;
using Repart.Common.Exceptions;

namespace Repart.Services.Intervenants.Domain.Models
{
    public class Intervenant
    {
        public Guid Id { get; protected set; }
        public string Firstname { get; protected set; }
        public string Lastname { get; protected set; }
        public string Initiales { get; protected set; }
        public Guid FormationId { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        public Intervenant(string firstname, string lastname, string initiales, Guid formationId)
        {
            if(string.IsNullOrEmpty(firstname))
                throw new RepartException("intervenant_firstname_empty",
                    "Le prénom de l'intervenant est vide.");

            if(string.IsNullOrEmpty(lastname))
                throw new RepartException("intervenant_lastname_empty",
                    "Le nom de l'intervenant est vide.");

            Id = Guid.NewGuid();
            Firstname = firstname;
            Lastname = lastname;
            Initiales = initiales;
            FormationId = formationId;
            CreatedAt = DateTime.UtcNow;
        }

        public void ChangeFormation(Guid formationId)
        {
            if(FormationId == formationId)
                throw new RepartException("intervenant_already_have_formation",
                    $"L'intervenant ({Id} possède déjà cette formation ({formationId}).)");

            FormationId = formationId;
        }
    }
}
