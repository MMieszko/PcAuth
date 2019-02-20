using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using PortaCapena.Authentication.NetCore.Extensions;

namespace PortaCapena.Authentication.NetCore.Core
{
    public class PcDefaultPolicy : AuthorizationPolicy
    {
        private PcDefaultPolicy(IEnumerable<IAuthorizationRequirement> requirements, IEnumerable<string> authenticationSchemes)
            : base(requirements, authenticationSchemes)
        {
        }

        public static PcDefaultPolicy Create => new PcDefaultPolicy(new[] { new PcDefaultAuthorizationRequirement() }, new[] { Constants.DefaultAuthorizationScheme });
    }
}