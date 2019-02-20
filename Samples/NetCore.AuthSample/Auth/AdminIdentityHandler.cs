using Microsoft.AspNetCore.Authorization;
using PortaCapena.Authentication.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortaCapena.Authentication.NetCore.Core;

namespace NetCore.AuthSample.Auth
{
    //Optional
    public class AdminIdentityHandler : PcIdentityHandler<AdminRole>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PcIdentityRequirement<AdminRole> requirement)
        {
            return base.HandleRequirementAsync(context, requirement);
        }

        public override Task OnUnauthorizedAsync(string message)
        {
            return base.OnUnauthorizedAsync(message);
        }
    }
}
