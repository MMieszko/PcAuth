using Microsoft.AspNetCore.Http;
using PortaCapena.Authentication.NetCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCoreSample.Middlewares
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

        public override Task Invoke(HttpContext context)
        {
            return base.Invoke(context);
        }

        public override Task SetClaimsPrincipalAsync(ClaimsPrincipal principal)
        {
            return base.SetClaimsPrincipalAsync(principal);
        }
    }
}
