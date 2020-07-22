using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServerApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", config =>
                {
                    //show the api how to connect to your identity server
                    config.Authority = "https://localhost:44338/";  //telling our api where it has to pass any "access_token" it receives from client requests, for validation i.e: where to send its OpenIdConnect calls (that will be to our Identity Server).
                    config.Audience = "ServerApi"; //identifying what resource is trying to pass this token for authentication, so we know that its not a random thing that's trying to validate a token, but rather its an API that we already know about.
                });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
