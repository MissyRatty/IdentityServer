using IdentityServer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityServerDbContext>(config =>
            {
                config.UseInMemoryDatabase("Memory");
            });

            //register identity services
            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<IdentityServerDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config => 
            {

                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/Authentication/Login";
            });

            //this will include AuthN & AuthZ : This provides us with the infrastructure for OAuth & OpenID Connect. (so we can implement OAuth Flows)
            //you can register clients & api resources with a database (you can have inmemory database : inmemory api resources (apis), inmemory clients.
            services.AddIdentityServer()
                .AddAspNetIdentity<IdentityUser>()
                .AddInMemoryApiResources(Configuration.GetApis())
                .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
                .AddInMemoryClients(Configuration.GetClients())       // provide list of clients that we would be issuing tokens to.
                .AddDeveloperSigningCredential();                    //this will generate a certificate to sign the generated tokens

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //add identity server middleware
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
