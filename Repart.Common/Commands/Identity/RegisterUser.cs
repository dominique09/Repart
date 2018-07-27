using System;
using System.Collections.Generic;
using System.Text;

namespace Repart.Common.Commands.Identity
{
    public class RegisterUser : ICommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> Roles { get; set; }
    }
}
