using System;
using System.Collections.Generic;
using System.Text;

namespace Repart.Common.Events.Identity
{
    public class Role : IEvent
    {
        public Guid Id { get; }
        public string Name { get; }

        protected Role(){}

        public Role(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
