using System;
using System.Collections.Generic;

namespace Repart.Common.Auth
{
    public interface IJwtHandler
    {
        JsonWebToken Create(Guid userId, IEnumerable<string> roles);
    }
}
