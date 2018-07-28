using System;
using System.Collections.Generic;
using System.Text;

namespace Repart.Common.Events.Formation
{
    public class FormationWithHistory
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Abreviation { get; }
        public IEnumerable<FormationHistory> Histories { get; }

        public FormationWithHistory(Guid id, string name, string abreviation, IEnumerable<FormationHistory> histories)
        {
            Id = id;
            Name = name;
            Abreviation = abreviation;
            Histories = histories;
        }
    }
}
