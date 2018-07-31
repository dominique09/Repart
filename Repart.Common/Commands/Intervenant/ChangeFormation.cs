using System;
using System.Collections.Generic;
using System.Text;

namespace Repart.Common.Commands.Intervenant
{
    public class ChangeFormation : ICommand
    {
        public Guid IntervenantId { get; }
        public Guid FormationId { get; }

        protected ChangeFormation(){}

        public ChangeFormation(Guid intervenantId, Guid formationId)
        {
            IntervenantId = intervenantId;
            FormationId = formationId;
        }
    }
}
