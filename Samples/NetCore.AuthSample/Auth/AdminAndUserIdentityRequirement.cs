using System.Collections.Generic;
using PortaCapena.Authentication.NetCore.Abstraction;
using PortaCapena.Authentication.NetCore.Core;

namespace NetCore.AuthSample.Auth
{
    public class AdminAndUserIdentityRequirement : PcMultiIdentityRequirement
    {
        public AdminAndUserIdentityRequirement(IList<Role> roles) : base(roles)
        {
        }
    }
}