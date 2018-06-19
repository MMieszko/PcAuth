using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PortaCapena.Authentication.Core;

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

        /// <summary>
        /// Throws <see cref="AuthException"/> with given message
        /// </summary>
        /// <param name="message">Exception message</param>
        public virtual Task OnUnauthorizedAsync(string message)
        {
            throw new AuthException(message);
        }
    }
}
