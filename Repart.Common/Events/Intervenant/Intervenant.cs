using System;

namespace Repart.Common.Events.Intervenant
{
    public class Intervenant : IEvent
    {
        public Guid Id { get; }
        public string Firstname { get; }
        public string Lastname { get; }
        public string Initiales { get; }
        public Guid FormationId { get; }
        public DateTime CreatedAt { get; }

        protected Intervenant(){}

        public Intervenant(Guid id, string firstname, string lastname, string initiales, Guid formationId,
            DateTime createdAt)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Initiales = initiales;
            FormationId = formationId;
            CreatedAt = createdAt;
        }
    }
}
