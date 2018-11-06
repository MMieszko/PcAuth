﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net;
using PortaCapena.Authentication.NetCore.Abstraction;
using PortaCapena.Authentication.NetCore.Configuration;
using PortaCapena.Authentication.NetCore.Exceptions;
using PortaCapena.Authentication.NetCore.Extensions;

namespace PortaCapena.Authentication.NetCore
{
    public class PcIdentityMiddleware : ITokenReader, ITokenRefresher
    {
        protected IServiceProvider ServiceProvider { get; private set; }
        protected HttpContext HttpContext { get; private set; }
        protected RequestDelegate Next { get; }

        public PcIdentityMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public virtual async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            HttpContext = context;

            var token = context.Request.Headers[TokenManager.TokenOptions.TokenName];

            if (string.IsNullOrEmpty(token))
            {
                await Next(context);
                return;
            }

            ClaimsPrincipal claimsPrinicipal;

            try
            {
                claimsPrinicipal = TokenManager.Read(token);
            }
            catch (AuthException ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await HttpContext.Response.WriteAsync("Unauthorized." + ex.Message);
                return;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error while trying to read token", ex);
            }

            await FillHttpContextWithIdentity(claimsPrinicipal);
            await Next(context);

            if (!TokenManager.TokenOptions.AutoRefresh) return;

            await RefreshTokenAsync(await CreateRefreshedToken(claimsPrinicipal.Claims));
        }

        protected virtual Task<string> CreateRefreshedToken(IEnumerable<Claim> claimsPrincipal)
        {
            return Task.FromResult(TokenManager.Create(claimsPrincipal));
        }

        /// <summary>
        /// Sets given <see cref="ClaimsPrincipal" into <see cref="HttpContext.User"/>
        /// Also sets <see cref="Claims.UserId"/> claim into <see cref="HttpContext.Items"/> as UserId key/>
        /// </summary>
        /// <param name="principal">Current user principals</param>
        public virtual Task FillHttpContextWithIdentity(ClaimsPrincipal principal)
        {
            HttpContext.User = principal;
            HttpContext.Items["UserId"] = principal.Claims.GetUserIdValue();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Put given newToken into response header with given exchange token name set in <see cref="ITokenOptionsBuilder"/>
        /// </summary>
        /// <param name="newToken">New generated token</param>
        public virtual Task RefreshTokenAsync(string newToken)
        {
            HttpContext.Response.Headers.Add(TokenManager.TokenOptions.ExchangeTokenName, newToken);
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", TokenManager.TokenOptions.ExchangeTokenName);

            return Task.CompletedTask;
        }
    }
}
