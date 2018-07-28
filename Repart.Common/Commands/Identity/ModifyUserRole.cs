using System;

namespace Repart.Common.Commands.Identity
{
    public class ModifyUserRole : ICommand
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
