using System;
using System.Collections.Generic;

namespace Repart.Common.Events.Identity
{
    public class User : IEvent
    {
        public string Email { get; }
        public string Name { get; }
        public IEnumerable<Role> Roles { get; }
        public DateTime CreatedAt { get; }

        protected User(){}

        public User(string email, string name, IEnumerable<Role> roles, DateTime createdAt)
        {
            Email = email;
            Name = name;
            Roles = roles;
            CreatedAt = createdAt;
        }
    }
}
