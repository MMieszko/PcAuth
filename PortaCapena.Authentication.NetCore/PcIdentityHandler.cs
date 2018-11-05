using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PortaCapena.Authentication.NetCore.Abstraction;
using PortaCapena.Authentication.NetCore.Extensions;

namespace PortaCapena.Authentication.NetCore
{
    public class PcIdentityHandler<TRole> : AuthorizationHandler<PcIdentityRequirement<TRole>>, ITokenValidator
        where TRole : Role
    {
        protected AuthorizationHandlerContext AuthContext { get; private set; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PcIdentityRequirement<TRole> requirement)
        {
            AuthContext = context;

            var roleValues = context.User.Claims.GetRoleValues();

            if (string.IsNullOrEmpty(roleValues))
                return OnUnauthorizedAsync("No roles found for given token");

            var roles = roleValues.Split(',');

            if (roles.All(x => x != requirement.Role.ToString()))
                return OnUnauthorizedAsync($"Not found {requirement.Role.ToString()} in current user roles");

            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task OnUnauthorizedAsync(string message)
        {
            throw new AuthException(message);
        }
    }
}
