using Microsoft.AspNetCore.Authorization;
using System;

namespace PortaCapena.Authentication.NetCore
{
    public class PcIdentityRequirement<TRole> : IAuthorizationRequirement
        where TRole : Role
    {
        public TRole Role { get; }

        public PcIdentityRequirement()
        {
            Role = Activator.CreateInstance<TRole>();
        }
    }
}
