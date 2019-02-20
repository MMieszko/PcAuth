using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PortaCapena.Authentication.NetCore;
using PortaCapena.Authentication.NetCore.Core;

namespace NetCore.AuthSample.Middlewares
{
    //Optional
    public class MyIdentityMiddleware : PcIdentityMiddleware
    {
        public MyIdentityMiddleware(RequestDelegate next)
            : base(next)
        {
        }

        public override Task RefreshTokenAsync(string newToken)
        {
            return base.RefreshTokenAsync(newToken);
        }

        public override Task Invoke(HttpContext context, IServiceProvider serviceProvider)
        {
            return base.Invoke(context, serviceProvider);
        }

        public override Task FillHttpContextWithIdentity(ClaimsPrincipal principal)
        {
            return base.FillHttpContextWithIdentity(principal);
        }
    }
}
