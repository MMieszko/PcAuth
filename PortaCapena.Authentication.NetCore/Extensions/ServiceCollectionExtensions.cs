using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PortaCapena.Authentication.NetCore.Abstraction;
using PortaCapena.Authentication.NetCore.Configuration;
using PortaCapena.Authentication.NetCore.Core;
using PortaCapena.Authentication.NetCore.Exceptions;

namespace PortaCapena.Authentication.NetCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds default policy to handle <see cref="AuthorizeAttribute"/> without providing policy name.
        /// This allow to pass any request which has valid token - no roles validation.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultPcIdentityPolicy(this IServiceCollection @this)
        {
            var defaultPolicy = PcDefaultPolicy.Create;

            @this.AddScoped<IAuthorizationHandler, DefaultAuthorizationHandler>();
            @this.AddAuthorization(options => options.AddPolicy(Constants.DefaultPolicyName, defaultPolicy));

            return @this;
        }

        /// <summary>
        /// Adds singleton authorization handler into <see cref="IServiceCollection"/> and add policy with given <see cref="PcIdentityRequirement{TRole}"/>/>"/>
        /// In order to use the policy use <see cref="AuthorizeAttribute"/> with name of policy
        /// </summary>
        /// <typeparam name="TRole">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="TRequirement">Requirement must inherit <see cref="PcIdentityRequirement{TRole}"/></typeparam>
        /// <typeparam name="THandler">Authorization handler must inherit<see cref="PcIdentityHandler{TRole}"/></typeparam>
        /// <param name="this"><see cref="IServiceCollection"/></param>
        /// <param name="policyName">Name of policy which is used in <see cref="Authorize"/> as policy parameter</param>
        /// <param name="injectionType">Registration of given handler in ASP.NET Core DI. By default its scoped</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddPcIdentityPolicy<TRole, TRequirement, THandler>(this IServiceCollection @this, string policyName, AuthorizationHandlerInjectionType injectionType = AuthorizationHandlerInjectionType.Scoped)
            where TRole : Role, new()
            where TRequirement : PcIdentityRequirement<TRole>
            where THandler : PcIdentityHandler<TRole>
        {
            var requirement = Activator.CreateInstance<TRequirement>();

            if (injectionType == AuthorizationHandlerInjectionType.Scoped)
                @this.AddScoped<IAuthorizationHandler, THandler>();
            else
                @this.AddSingleton<IAuthorizationHandler, THandler>();

            @this.AddAuthorization(options => options.AddPolicy(policyName, policy => policy.Requirements.Add(requirement)));

            return @this;
        }

        /// <summary>
        /// Adds singleton authorization handler into <see cref="IServiceCollection"/> and add policy with given <see cref="PcIdentityRequirement{TRole}"/>/>"/>
        /// In order to use the policy use <see cref="AuthorizeAttribute"/> with name of policy
        ///         /// The policy by default check if any of given roles fits the Policy
        /// </summary>
        /// <typeparam name="TRole">Role must inherit <see cref="Role"/></typeparam>
        /// <param name="this"><see cref="IServiceCollection"/></param>
        /// <param name="policyName">Name of policy which is used in <see cref="Authorize"/> as policy parameter</param>
        /// <param name="injectionType">Registration of given handler in ASP.NET Core DI. By default its scoped</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddPcIdentityPolicy<TRole>(this IServiceCollection @this, string policyName, AuthorizationHandlerInjectionType injectionType = AuthorizationHandlerInjectionType.Scoped)
            where TRole : Role, new()
        {
            var requirement = new PcIdentityRequirement<TRole>();

            if (injectionType == AuthorizationHandlerInjectionType.Scoped)
                @this.AddScoped<IAuthorizationHandler, PcIdentityHandler<TRole>>();
            else
                @this.AddSingleton<IAuthorizationHandler, PcIdentityHandler<TRole>>();

            @this.AddAuthorization(options => options.AddPolicy(policyName, policy => policy.Requirements.Add(requirement)));

            return @this;
        }

        /// <summary>
        /// Adds singleton authorization handler into <see cref="IServiceCollection"/> and add policy with given <see cref="PcIdentityRequirement{TRole}"/>/>"/>
        /// In order to use the policy use <see cref="AuthorizeAttribute"/> with name of policy
        ///         /// The policy by default check if any of given roles fits the Policy
        /// </summary>
        /// <typeparam name="T1">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T2">Role must inherit <see cref="Role"/></typeparam>
        /// <param name="this"><see cref="IServiceCollection"/></param>
        /// <param name="policyName">Name of policy which is used in <see cref="Authorize"/> as policy parameter</param>
        /// <param name="injectionType">Registration of given handler in ASP.NET Core DI. By default its scoped</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddPcMultiRoleIdentityPolicy<T1, T2>(this IServiceCollection @this, string policyName, AuthorizationHandlerInjectionType injectionType = AuthorizationHandlerInjectionType.Scoped)
            where T1 : Role, new()
            where T2 : Role, new()
        {
            return AddPcMultiRoleIdentityPolicy(@this, policyName, new List<Role> { new T1(), new T2() }, injectionType);
        }

        /// <summary>
        /// Adds singleton authorization handler into <see cref="IServiceCollection"/> and add policy with given <see cref="PcIdentityRequirement{TRole}"/>/>"/>
        /// In order to use the policy use <see cref="AuthorizeAttribute"/> with name of policy
        ///         /// The policy by default check if any of given roles fits the Policy
        /// </summary>
        /// <typeparam name="T1">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T2">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T3">Role must inherit <see cref="Role"/></typeparam>
        /// <param name="this"><see cref="IServiceCollection"/></param>
        /// <param name="policyName">Name of policy which is used in <see cref="Authorize"/> as policy parameter</param>
        /// <param name="injectionType">Registration of given handler in ASP.NET Core DI. By default its scoped</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddPcMultiRoleIdentityPolicy<T1, T2, T3>(this IServiceCollection @this, string policyName, AuthorizationHandlerInjectionType injectionType = AuthorizationHandlerInjectionType.Scoped)
            where T1 : Role, new()
            where T2 : Role, new()
            where T3 : Role, new()
        {
            return AddPcMultiRoleIdentityPolicy(@this, policyName, new List<Role> { new T1(), new T2(), new T3() }, injectionType);
        }

        /// <summary>
        /// Adds singleton authorization handler into <see cref="IServiceCollection"/> and add policy with given <see cref="PcIdentityRequirement{TRole}"/>/>"/>
        /// In order to use the policy use <see cref="AuthorizeAttribute"/> with name of policy
        /// The policy by default check if any of given roles fits the Policy
        /// </summary>
        /// <typeparam name="T1">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T2">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T3">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T4">Role must inherit <see cref="Role"/></typeparam>
        /// <param name="this"><see cref="IServiceCollection"/></param>
        /// <param name="policyName">Name of policy which is used in <see cref="Authorize"/> as policy parameter</param>
        /// <param name="injectionType">Registration of given handler in ASP.NET Core DI. By default its scoped</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddPcMultiRoleIdentityPolicy<T1, T2, T3, T4>(this IServiceCollection @this, string policyName, AuthorizationHandlerInjectionType injectionType = AuthorizationHandlerInjectionType.Scoped)
            where T1 : Role, new()
            where T2 : Role, new()
            where T3 : Role, new()
            where T4 : Role, new()
        {
            return AddPcMultiRoleIdentityPolicy(@this, policyName, new List<Role> { new T1(), new T2(), new T3(), new T4() }, injectionType);
        }

        /// <summary>
        /// Adds singleton authorization handler into <see cref="IServiceCollection"/> and add policy with given <see cref="PcIdentityRequirement{TRole}"/>/>"/>
        /// In order to use the policy use <see cref="AuthorizeAttribute"/> with name of policy
        /// The policy by default check if any of given roles fits the Policy
        /// </summary>
        /// <typeparam name="T1">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T2">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T3">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T4">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T5">Role must inherit <see cref="Role"/></typeparam>
        /// <param name="this"><see cref="IServiceCollection"/></param>
        /// <param name="policyName">Name of policy which is used in <see cref="Authorize"/> as policy parameter</param>
        /// <param name="injectionType">Registration of given handler in ASP.NET Core DI. By default its scoped</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddPcMultiRoleIdentityPolicy<T1, T2, T3, T4, T5>(this IServiceCollection @this, string policyName, AuthorizationHandlerInjectionType injectionType = AuthorizationHandlerInjectionType.Scoped)
            where T1 : Role, new()
            where T2 : Role, new()
            where T3 : Role, new()
            where T4 : Role, new()
            where T5 : Role, new()
        {
            return AddPcMultiRoleIdentityPolicy(@this, policyName, new List<Role> { new T1(), new T2(), new T3(), new T4(), new T5() }, injectionType);
        }

        /// <summary>
        /// Adds singleton authorization handler into <see cref="IServiceCollection"/> and add policy with given <see cref="PcIdentityRequirement{TRole}"/>/>"/>
        /// In order to use the policy use <see cref="AuthorizeAttribute"/> with name of policy
        /// The policy by default check if any of given roles fits the Policy
        /// </summary>
        /// <typeparam name="T1">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T2">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T3">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T4">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T5">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T6">Role must inherit <see cref="Role"/></typeparam>
        /// <param name="this"><see cref="IServiceCollection"/></param>
        /// <param name="policyName">Name of policy which is used in <see cref="Authorize"/> as policy parameter</param>
        /// <param name="injectionType">Registration of given handler in ASP.NET Core DI. By default its scoped</param>
        public static IServiceCollection AddPcMultiRoleIdentityPolicy<T1, T2, T3, T4, T5, T6>(this IServiceCollection @this, string policyName, AuthorizationHandlerInjectionType injectionType = AuthorizationHandlerInjectionType.Scoped)
            where T1 : Role, new()
            where T2 : Role, new()
            where T3 : Role, new()
            where T4 : Role, new()
            where T5 : Role, new()
            where T6 : Role, new()
        {
            return AddPcMultiRoleIdentityPolicy(@this, policyName, new List<Role> { new T1(), new T2(), new T3(), new T4(), new T5(), new T6() }, injectionType);
        }

        /// <summary>
        /// Adds singleton authorization handler into <see cref="IServiceCollection"/> and add policy with given <see cref="PcIdentityRequirement{TRole}"/>/>"/>
        /// In order to use the policy use <see cref="AuthorizeAttribute"/> with name of policy
        /// The policy by default check if any of given roles fits the Policy
        /// </summary>
        /// <typeparam name="T1">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T2">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T3">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T4">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T5">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T6">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T7">Role must inherit <see cref="Role"/></typeparam>
        /// <param name="this"><see cref="IServiceCollection"/></param>
        /// <param name="policyName">Name of policy which is used in <see cref="Authorize"/> as policy parameter</param>
        /// <param name="injectionType">Registration of given handler in ASP.NET Core DI. By default its scoped</param>
        public static IServiceCollection AddPcMultiRoleIdentityPolicy<T1, T2, T3, T4, T5, T6, T7>(this IServiceCollection @this, string policyName, AuthorizationHandlerInjectionType injectionType = AuthorizationHandlerInjectionType.Scoped)
            where T1 : Role, new()
            where T2 : Role, new()
            where T3 : Role, new()
            where T4 : Role, new()
            where T5 : Role, new()
            where T6 : Role, new()
            where T7 : Role, new()
        {
            return AddPcMultiRoleIdentityPolicy(@this, policyName, new List<Role> { new T1(), new T2(), new T3(), new T4(), new T5(), new T6(), new T7() }, injectionType);
        }

        /// <summary>
        /// Adds singleton authorization handler into <see cref="IServiceCollection"/> and add policy with given <see cref="PcIdentityRequirement{TRole}"/>/>"/>
        /// In order to use the policy use <see cref="AuthorizeAttribute"/> with name of policy
        /// The policy by default check if any of given roles fits the Policy
        /// </summary>
        /// <typeparam name="T1">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T2">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T3">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T4">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T5">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T6">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T7">Role must inherit <see cref="Role"/></typeparam>
        /// <typeparam name="T8">Role must inherit <see cref="Role"/></typeparam>
        /// <param name="this"><see cref="IServiceCollection"/></param>
        /// <param name="policyName">Name of policy which is used in <see cref="Authorize"/> as policy parameter</param>
        /// <param name="injectionType">Registration of given handler in ASP.NET Core DI. By default its scoped</param>
        public static IServiceCollection AddPcMultiRoleIdentityPolicy<T1, T2, T3, T4, T5, T6, T7, T8>(this IServiceCollection @this, string policyName, AuthorizationHandlerInjectionType injectionType = AuthorizationHandlerInjectionType.Scoped)
            where T1 : Role, new()
            where T2 : Role, new()
            where T3 : Role, new()
            where T4 : Role, new()
            where T5 : Role, new()
            where T6 : Role, new()
            where T7 : Role, new()
            where T8 : Role, new()

        {
            return AddPcMultiRoleIdentityPolicy(@this, policyName, new List<Role> { new T1(), new T2(), new T3(), new T4(), new T5(), new T6(), new T7(), new T8() }, injectionType);
        }

        internal static IServiceCollection AddPcMultiRoleIdentityPolicy(this IServiceCollection @this, string policyName, IList<Role> roles, AuthorizationHandlerInjectionType injectionType = AuthorizationHandlerInjectionType.Scoped)
        {
            var requirement = new PcMultiIdentityRequirement(roles);

            if (injectionType == AuthorizationHandlerInjectionType.Scoped)
                @this.AddScoped<IAuthorizationHandler, PcMultiIdentityHandler>();
            else
                @this.AddSingleton<IAuthorizationHandler, PcMultiIdentityHandler>();

            @this.AddAuthorization(options => options.AddPolicy(policyName, policy => policy.Requirements.Add(requirement)));

            return @this;
        }

        /// <summary>
        /// Adds given <see cref="PcIdentityMiddleware"/> into the pipeline. Also setup the <see cref="PcAuthInitializer"/> with given token options
        /// </summary>
        /// <typeparam name="TMiddleware"></typeparam>
        /// <param name="this">IApplicationBuilder</param>
        /// <param name="tokenOptions">Token options built with <see cref="ITokenOptionsBuilder"/></param>
        /// <returns>self</returns>
        public static IApplicationBuilder UsePcIdentityMiddleware<TMiddleware>(this IApplicationBuilder @this, TokenOptions tokenOptions)
            where TMiddleware : PcIdentityMiddleware
        {
            PcAuthInitializer.Initialize(tokenOptions);

            @this.UseMiddleware<TMiddleware>();

            @this.UseAuthentication();

            return @this;
        }

        /// <summary>
        /// Adds anonymous middleware into the pipeline with handle the exception with typeof <see cref="AuthException"/>
        /// </summary>
        /// <param name="this">IApplicationBuilder</param>
        /// <param name="func">Delegate to handle the exception</param>
        /// <returns>self</returns>
        public static IApplicationBuilder UsePcIdentityExceptionHandler(this IApplicationBuilder @this, Func<HttpContext, AuthException, Task> func)
        {
            @this.UseExceptionHandler(options => options.Run(async (context) =>
            {
                var feature = context.Features.Get<IExceptionHandlerFeature>();
                if (feature.Error.GetBaseException() is AuthException ex)
                    await func.Invoke(context, ex);
            }));

            return @this;
        }

        #region - Depracated -

        [Obsolete("Instead use AddPcIdentityPolicy extension method")]
        public static IServiceCollection SetIdentityRequirements<TRole, TRequirement, THandler>(this IServiceCollection @this, string name)
            where TRole : Role, new()
            where TRequirement : PcIdentityRequirement<TRole>
            where THandler : PcIdentityHandler<TRole>
        {
            throw new NotImplementedException($"This method is obsolete and will be removed in further version. Use {nameof(AddPcIdentityPolicy)} extension method instead.");
        }

        [Obsolete("Instead use UsePcIdentityMiddleware extension method")]
        public static IApplicationBuilder SetIdentityMiddleware<TMiddleware>(this IApplicationBuilder @this, TokenOptions tokenOptions)
            where TMiddleware : PcIdentityMiddleware
        {
            throw new NotImplementedException($"This method is obsolete and will be removed in further version. Use {nameof(UsePcIdentityMiddleware)} extension method instead.");
        }

        [Obsolete("Instead use UsePcIdentityExceptionHandler extension method")]
        public static IApplicationBuilder HandleAuthException(this IApplicationBuilder @this, Func<HttpContext, AuthException, Task> func)
        {
            throw new NotImplementedException($"This method is obsolete and will be removed in further version. Use {nameof(UsePcIdentityExceptionHandler)} extension method instead.");
        }

        #endregion
    }
}
