using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AuthSample.Auth;
using PortaCapena.Authentication.NetCore;
using PortaCapena.Authentication.NetCore.Configuration;

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

            ///With own classes
            //services.SetIdentityRequirements<AdminRole, PcIdentityRequirement<AdminRole>, PcIdentityHandler<AdminRole>>(nameof(AdminRequirement));

            ///Built in classes
            services.SetIdentityRequirements<AdminRole, AdminRequirement, AdminIdentityHandler>(nameof(AdminRequirement));

            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            

            ///Built in
            app.SetIdentityMiddleware<PcIdentityMiddleware>(new TokenOptionsBuilder().SetTokenName("access_token")
                                                        .SetSecretKey("this is my custom Secret key for authnetication")
                                                        .SetExpiration(TimeSpan.FromMinutes(15))
                                                        .SetAutoRefresh(false)
                                                        .Build());


            app.UseCors(config => config.WithHeaders(TokenManager.TokenOptions.ExchangeTokenName).AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            ///Custom
            //app.SetIdentityMiddleware<MyIdentityMiddleware>(tokenOptions);

            app.HandleAuthException(async (ctx, exc) =>
            {
                ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await ctx.Response.WriteAsync(exc.Message);

            });

            var token = TokenManager.Create(126, new AdminRole());


            app.UseMvc();
        }
    }
}
