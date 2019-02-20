using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AuthSample.Auth;
using PortaCapena.Authentication.NetCore.Configuration;
using PortaCapena.Authentication.NetCore.Core;
using PortaCapena.Authentication.NetCore.Extensions;

namespace NetCore.AuthSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            //With just role
            services.AddPcIdentityPolicy<AdminRole>("AdminPolicy")
                    .AddPcIdentityPolicy<UserRole>("UserPolicy")
                    .AddDefaultPcIdentityPolicy();

            //With own classes
            //services.AddPcIdentityPolicy<AdminRole, PcIdentityRequirement<AdminRole>, PcIdentityHandler<AdminRole>>(nameof(AdminRequirement))
            //        .AddDefaultPcIdentityPolicy();

            //Built in classes
            //services.AddPcIdentityPolicy<AdminRole, AdminRequirement, AdminIdentityHandler>(nameof(AdminRequirement))
            //        .AddDefaultPcIdentity();

            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



            //Built in
            app.UsePcIdentityMiddleware<PcIdentityMiddleware>(TokenOptionsBuilder.Create("access_token")
                                                        .SetSecretKey("this is my custom Secret key for authnetication")
                                                        .SetExpiration(TimeSpan.FromMinutes(15))
                                                        .SetAutoRefresh(false)
                                                        .Build());


            app.UseCors(config => config.WithHeaders(TokenManager.TokenOptions.ExchangeTokenName).AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            //Custom
            //app.UsePcIdentityMiddleware<MyIdentityMiddleware>(tokenOptions);

            app.UsePcIdentityExceptionHandler(async (ctx, exc) =>
            {
                ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await ctx.Response.WriteAsync(exc.Message);

            });

            var token = TokenManager.Create(126, new AdminRole());


            app.UseMvc();
        }
    }
}
