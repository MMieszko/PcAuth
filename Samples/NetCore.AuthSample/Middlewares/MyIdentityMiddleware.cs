using Microsoft.AspNetCore.Http;
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
    }
}
