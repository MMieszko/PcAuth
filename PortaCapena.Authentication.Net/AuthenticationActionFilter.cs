using System.Net;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using PortaCapena.Authentication.Core;

namespace PortaCapena.Authentication.Net
{
    public class AuthenticationActionFilter : ActionFilterAttribute, ITokenValidator, ITokenRefresher, ITokenReader
    {
        public string[] Roles { get; }
        protected HttpActionExecutedContext ActionExecutedContext { get; set; }

        public AuthenticationActionFilter(params string[] roles)
        {
            this.Roles = roles;
        }

        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            await base.OnActionExecutingAsync(actionContext, cancellationToken);

            string token = null;

            if (actionContext.Request.Headers.TryGetValues(TokenManager.TokenOptions.TokenName, out var headerValues))
                token = headerValues.FirstOrDefault();
            else
                await OnUnauthorizedAsync("Token was not found in request header");

            var principals = TokenManager.Read(token);

            var currentRoles = principals.Claims.GetRoleValues()?.Split(',');

            if (currentRoles == null)
            {
                await OnUnauthorizedAsync("No roles found in given token");
                return;
            }

            foreach (var requiredRole in Roles)
            {
                if (currentRoles.Contains(requiredRole)) continue;

                await OnUnauthorizedAsync($"Token do not have required role with value - {requiredRole}");
                return;
            }

            await SetClaimsPrincipalAsync(principals);
        }

        /// <summary>
        /// Throws <see cref="AuthException"/> with given message
        /// </summary>
        /// <param name="message">Exception message</param>
        public virtual Task OnUnauthorizedAsync(string message)
        {
            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// Sets given <see cref="ClaimsPrincipal" into <see cref="HttpContext.User"/>
        /// Also sets <see cref="Claims.UserId"/> claim into <see cref="HttpContext.Items"/> as UserId key/>
        /// </summary>
        /// <param name="principal">Current usser principals</param>
        public virtual Task SetClaimsPrincipalAsync(ClaimsPrincipal principal)
        {
            HttpContext.Current.User = principal;
            HttpContext.Current.Items["UserId"] = principal.Claims.GetUserIdValue();

            return Task.CompletedTask;
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);

            ActionExecutedContext = actionExecutedContext;

            if (!TokenManager.TokenOptions.AutoRefresh) return Task.CompletedTask;

            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            var newToken = TokenManager.Create(identity.Claims);

            return RefreshTokenAsync(newToken);
        }

        /// <summary>
        /// Put given newToken into response header with given exchange token name set in <see cref="ITokenOptionsBuilder"/>
        /// </summary>
        /// <param name="newToken">New generated token</param>
        public Task RefreshTokenAsync(string newToken)
        {
            ActionExecutedContext.Response.Headers.Add(TokenManager.TokenOptions.ExchangeTokenName, newToken);
            ActionExecutedContext.Response.Headers.Add("Access-Control-Expose-Headers", TokenManager.TokenOptions.ExchangeTokenName);

            return Task.CompletedTask;
        }
    }
}