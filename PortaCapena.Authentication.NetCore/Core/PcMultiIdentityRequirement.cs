using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using PortaCapena.Authentication.NetCore.Abstraction;

namespace PortaCapena.Authentication.NetCore.Core
{

    public class PcMultiIdentityRequirement : IAuthorizationRequirement
    {
        public IList<Role> Roles { get; }

        public PcMultiIdentityRequirement(IList<Role> roles)
        {
            Roles = roles;
        }
    }
}