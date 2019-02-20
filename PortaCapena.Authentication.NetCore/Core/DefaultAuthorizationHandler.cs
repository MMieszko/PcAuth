using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PortaCapena.Authentication.NetCore.Core
{
    public class DefaultAuthorizationHandler : AuthorizationHandler<IAuthorizationRequirement>
    {
        protected  override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}