using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PortaCapena.Authentication.NetCore.Abstraction;
using PortaCapena.Authentication.NetCore.Exceptions;
using PortaCapena.Authentication.NetCore.Extensions;

namespace PortaCapena.Authentication.NetCore.Core
{
    public class PcIdentityHandler<TRole> : AuthorizationHandler<PcIdentityRequirement<TRole>>, ITokenValidator
        where TRole : Role, new()
    {
        protected AuthorizationHandlerContext AuthContext { get; private set; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PcIdentityRequirement<TRole> requirement)
        {
            AuthContext = context;

            var roleValues = context.User.Claims.GetRolesArray();

            if (roleValues == null)
                return OnUnauthorizedAsync(new UnauthorizedException("Unauthorized for given operation"));

            if (roleValues.All(x => x != requirement.Role.ToString()))
                return OnUnauthorizedAsync(new UnauthorizedException("Unauthorized for given operation"));

            context.Succeed(requirement);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Throws <see cref="AuthException"/> with provided message
        /// </summary>
        /// <param name="message">Exception message</param>
        public virtual Task OnUnauthorizedAsync(AuthException exception)
        {
            throw exception;
        }
    }
}
