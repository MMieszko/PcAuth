using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PortaCapena.Authentication.NetCore.Core;
using PortaCapena.Authentication.NetCore.Exceptions;

namespace WebApi.Core
{
    //Optional
    public class AdminIdentityHandler : PcIdentityHandler<AdminRole>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PcIdentityRequirement<AdminRole> requirement)
        {
            return base.HandleRequirementAsync(context, requirement);
        }

        public override Task OnUnauthorizedAsync(AuthException ex)
        {
            return base.OnUnauthorizedAsync(ex);
        }
    }
}
