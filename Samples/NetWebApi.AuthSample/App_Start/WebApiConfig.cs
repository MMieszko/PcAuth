using NetWebApi.AuthSample.Auth;
using PortaCapena.Authentication.Core;
using System;
using System.Web.Http;

namespace NetWebApi.AuthSample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            
            PcAuthInitializer.Initialize(new TokenOptionsBuilder().SetTokenName("Token")
                                            .SetSecretKey("this is my custom Secret key for authnetication")
                                            .SetExchangeTokenName("Exchanged")
                                            .SetExpiration(TimeSpan.FromMinutes(15))
                                            .Build());

            var adminToken = TokenManager.Create(12, Roles.AdminRole);
            var superAdminToken = TokenManager.Create(6234, Roles.SuperAdminRole);
            var bothRole = TokenManager.Create(124, Roles.AdminRole, Roles.SuperAdminRole);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
