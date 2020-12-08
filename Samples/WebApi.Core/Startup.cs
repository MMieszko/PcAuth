using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PortaCapena.Authentication.NetCore.Configuration;
using PortaCapena.Authentication.NetCore.Core;
using PortaCapena.Authentication.NetCore.Extensions;

namespace WebApi.Core
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
            services.AddControllers();

            //With just role
            services.AddPcIdentityPolicy<AdminRole>("AdminPolicy")
                .AddPcIdentityPolicy<UserRole>("UserPolicy")
                .AddPcIdentityPolicy<WorkerRole>("WorkerPolicy")
                .AddPcMultiRoleIdentityPolicy<AdminRole, UserRole>("AdminOrUserPolicy")
                .AddDefaultPcIdentityPolicy();

            //With own classes
            //services.AddPcIdentityPolicy<AdminRole, PcIdentityRequirement<AdminRole>, PcIdentityHandler<AdminRole>>(nameof(AdminRequirement))
            //        .AddDefaultPcIdentityPolicy();

            //Built in classes
            //services.AddPcIdentityPolicy<AdminRole, AdminRequirement, AdminIdentityHandler>(nameof(AdminRequirement))
            //        .AddDefaultPcIdentity();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
                //ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await ctx.Response.WriteAsync(exc.Message);

            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
