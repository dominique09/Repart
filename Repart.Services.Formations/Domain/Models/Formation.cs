using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repart.Common.Exceptions;

namespace Repart.Services.Formations.Domain.Models
{
    public class Formation
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Abreviation { get; protected set; }

        public Formation(string name, string abreviation)
        {
            if(string.IsNullOrEmpty(name))
                throw new RepartException("formation_name_empty",
                    $"Le nom de la formation est vide.");
            if(string.IsNullOrEmpty(abreviation))
                throw new RepartException("formation_abreviation_empty",
                    $"L'abreviation de la formation est vide.");
            if(abreviation.Length > 5)
                throw new RepartException("formation_abreviation_too_long",
                    $"L'abreviation doit avoir moins de 5 caractères.");

            Id = Guid.NewGuid();
            Name = name;
            Abreviation = abreviation.ToUpperInvariant();
        }
    }
}
