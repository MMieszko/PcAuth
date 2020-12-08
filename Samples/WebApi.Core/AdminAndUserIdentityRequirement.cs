using System.Collections.Generic;
using PortaCapena.Authentication.NetCore.Abstraction;
using PortaCapena.Authentication.NetCore.Core;

namespace WebApi.Core
{
    public class AdminAndUserIdentityRequirement : PcMultiIdentityRequirement
    {
        public AdminAndUserIdentityRequirement(IList<Role> roles) : base(roles)
        {
        }
    }
}