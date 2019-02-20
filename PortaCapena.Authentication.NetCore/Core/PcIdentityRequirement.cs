using System;
using Microsoft.AspNetCore.Authorization;
using PortaCapena.Authentication.NetCore.Abstraction;

namespace PortaCapena.Authentication.NetCore.Core
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
