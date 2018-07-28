using System;
using System.Collections.Generic;
using System.Text;

namespace Repart.Common.Events.Formation
{
    public class Formation
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Abreviation { get; }

        public Formation(Guid id, string name, string abreviation)
        {
            Id = id;
            Name = name;
            Abreviation = abreviation;
        }
    }
}
