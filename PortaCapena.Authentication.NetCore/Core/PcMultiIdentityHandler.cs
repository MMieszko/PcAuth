using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PortaCapena.Authentication.NetCore.Abstraction;
using PortaCapena.Authentication.NetCore.Exceptions;
using PortaCapena.Authentication.NetCore.Extensions;

namespace PortaCapena.Authentication.NetCore.Core
{
    public class PcMultiIdentityHandler : AuthorizationHandler<PcMultiIdentityRequirement>, ITokenValidator
    {
        protected AuthorizationHandlerContext AuthContext { get; private set; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PcMultiIdentityRequirement requirement)
        {
            AuthContext = context;
            var claimRoles = context.User.Claims.GetRolesArray();

            if (claimRoles == null)
                return OnUnauthorizedAsync(new UnauthorizedException("Unauthorized for given operation"));

            if (!requirement.Roles.Any(role => claimRoles.Contains(role.ToString())))
                return OnUnauthorizedAsync(new UnauthorizedException("Unauthorized for given operation"));

            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        public virtual Task OnUnauthorizedAsync(AuthException exception)
        {
            throw exception;
        }
    }
}