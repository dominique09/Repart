using System;
using System.Collections.Generic;
using System.Text;

namespace Repart.Common.Commands.Intervenant
{
    public class CreateIntervenant : ICommand
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Guid FormationId { get; set; }
    }
}
