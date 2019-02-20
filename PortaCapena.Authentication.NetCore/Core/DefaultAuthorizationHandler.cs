using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PortaCapena.Authentication.NetCore.Exceptions;
using PortaCapena.Authentication.NetCore.Extensions;

namespace PortaCapena.Authentication.NetCore.Core
{
    public class DefaultAuthorizationHandler : AuthorizationHandler<IAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
        {
            if (context.User?.Claims == null || !context.User.Claims.Any() || context.User.GetUserId() == null)
                throw new UnauthorizedException("Unauthorized for given operation");

            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}