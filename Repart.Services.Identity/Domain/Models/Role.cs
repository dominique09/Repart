using System;
using Repart.Common.Exceptions;

namespace Repart.Services.Identity.Domain.Models
{
    public class Role
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }

        protected Role(){}

        public Role(string name)
        {
            if(string.IsNullOrEmpty(name))
                throw new RepartException("empty_role_name",
                    $"Le nom du role ne peut pas être vide.");

            Name = name;
        }
    }
}
