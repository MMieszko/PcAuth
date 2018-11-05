using Microsoft.AspNetCore.Authorization;
using System;
using PortaCapena.Authentication.NetCore.Abstraction;

namespace PortaCapena.Authentication.NetCore
{
    public class PcIdentityRequirement<TRole> : IAuthorizationRequirement
        where TRole : Role, new()
    {
        public TRole Role { get; }

        public PcIdentityRequirement()
        {
            Role = Activator.CreateInstance<TRole>();
        }
    }
}
