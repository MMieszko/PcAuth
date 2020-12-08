using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PortaCapena.Authentication.NetCore.Core;

namespace WebApi.Core
{
    //Optional
    public class MyIdentityMiddleware : PcIdentityMiddleware
    {
        public MyIdentityMiddleware(RequestDelegate next)
            : base(next)
        {
        }

        public override Task Invoke(HttpContext context, IServiceProvider serviceProvider)
        {
            return base.Invoke(context, serviceProvider);
        }
    }
}
