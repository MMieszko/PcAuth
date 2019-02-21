using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PortaCapena.Authentication.NetCore.Abstraction;
using PortaCapena.Authentication.NetCore.Exceptions;
using PortaCapena.Authentication.NetCore.Extensions;

namespace PortaCapena.Authentication.NetCore.Core
{
    public class PcIdentityMiddleware : ITokenReader, ITokenRefresher
    {
        protected IServiceProvider ServiceProvider { get; private set; }
        protected RequestDelegate Next { get; }

        public PcIdentityMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public virtual async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;

            var token = context.Request.Headers[TokenManager.TokenOptions.TokenName];

            if (string.IsNullOrEmpty(token))
            {
                await Next(context);
                return;
            }
            try
            {
                var claimsPrincipal = TokenManager.Read(token);

                await FillHttpContextWithIdentity(claimsPrincipal, context);
                await Next(context);

                if (!TokenManager.TokenOptions.AutoRefresh) return;

                var refreshedToken = await CreateRefreshedToken(claimsPrincipal.Claims);

                await RefreshTokenAsync(refreshedToken, context);
            }
            catch (AuthException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized - " + ex.Message);
            }
        }

        protected virtual Task<string> CreateRefreshedToken(IEnumerable<Claim> claimsPrincipal)
        {
            return Task.FromResult(TokenManager.Create(claimsPrincipal));
        }

        /// <summary>
        /// Sets given <see cref="ClaimsPrincipal"/> into <see cref="HttpContext.User"/>
        /// Also sets <see cref="Claims.UserId"/> claim into <see cref="HttpContext.Items"/> as UserId key/>
        /// </summary>
        /// <param name="principal">Current user principals</param>
        /// <param name="context">Current http context</param>
        public virtual Task FillHttpContextWithIdentity(ClaimsPrincipal principal, HttpContext context)
        {
            context.User = principal;
            context.Items[Constants.UserId] = principal.Claims.GetUserIdValue();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Put given newToken into response header with given exchange token name set in <see cref="ITokenOptionsBuilder"/>
        /// </summary>
        /// <param name="newToken">New generated token</param>
        /// <param name="context">Current HttpContext</param>
        public virtual Task RefreshTokenAsync(string newToken, HttpContext context)
        {
            context.Response.Headers.Add(TokenManager.TokenOptions.ExchangeTokenName, newToken);
            context.Response.Headers.Add("Access-Control-Expose-Headers", TokenManager.TokenOptions.ExchangeTokenName);

            return Task.CompletedTask;
        }
    }
}
