using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PortaCapena.Authentication.NetCore
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds singleton authorization handler into <see cref="IServiceCollection"/> and add policy with given <see cref="PcIdentityRequirement{TRole}"/>/>"/>
        /// </summary>
        /// <typeparam name="TRole">Role must inheret <see cref="Role"/></typeparam>
        /// <typeparam name="TRequirement">Requirement must inheret <see cref="PcIdentityRequirement{TRole}"/></typeparam>
        /// <typeparam name="THandler">Authorization handler must inheret<see cref="PcIdentityHandler{TRole}"/></typeparam>
        /// <param name="this"><see cref="IServiceCollection"/></param>
        /// <param name="name">Name of policy which is used in <see cref="Authorize"/> as policy parameter</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection SetIdentityRequirements<TRole, TRequirement, THandler>(this IServiceCollection @this, string name)
            where TRole : Role
            where TRequirement : PcIdentityRequirement<TRole>
            where THandler : PcIdentityHandler<TRole>
        {
            var requirement = Activator.CreateInstance<TRequirement>();

            @this.AddSingleton<IAuthorizationHandler, THandler>();
            @this.AddAuthorization(options => options.AddPolicy(name, policy => policy.Requirements.Add(requirement)));

            return @this;
        }

        /// <summary>
        /// Adds given <see cref="PcIdentityMiddleware"/> into the pipeline. Also setup the <see cref="PcAuthInitializer"/> with given token options
        /// </summary>
        /// <typeparam name="TMiddleware"></typeparam>
        /// <param name="this">IApplicationBuilder</param>
        /// <param name="tokenOptions">Token options built with <see cref="ITokenOptionsBuilder"/></param>
        /// <returns>self</returns>
        public static IApplicationBuilder SetIdentityMiddleware<TMiddleware>(this IApplicationBuilder @this, TokenOptions tokenOptions)
            where TMiddleware : PcIdentityMiddleware
        {
            PcAuthInitializer.Initialize(tokenOptions);

            @this.UseMiddleware<TMiddleware>();

            @this.UseAuthentication();

            return @this;
        }

        /// <summary>
        /// Adds annymous middleware into the pipelile with handle the exception with typeof <see cref="AuthException"/>
        /// </summary>
        /// <param name="this">IApplicationBuilder</param>
        /// <param name="func">Delegate to handle the exception</param>
        /// <returns>self</returns>
        public static IApplicationBuilder HandleAuthException(this IApplicationBuilder @this, Func<HttpContext, AuthException, Task> func)
        {
            @this.UseExceptionHandler(options => options.Run(async (context) =>
            {
                var feature = context.Features.Get<IExceptionHandlerFeature>();
                if (feature.Error.GetBaseException() is AuthException ex)
                    await func.Invoke(context, ex);
            }));

            return @this;
        }
    }
}
