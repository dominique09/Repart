using System;
using System.Collections.Generic;

namespace Repart.Common.Events.Identity
{
    public class User : IEvent
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public DateTime CreatedAt { get; set; }

        protected User(){}

        public User(Guid id, string email, string name, bool active, IEnumerable<Role> roles, DateTime createdAt)
        {
            Id = id;
            Email = email;
            Name = name;
            Active = active;
            Roles = roles;
            CreatedAt = createdAt;
        }
    }
}
