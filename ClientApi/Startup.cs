using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace ClientApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", config =>
                {
                    //show the api how to connect to your identity server
                    config.Authority = "https://localhost:44338/";  //telling our api where it has to pass any "access_tokens" it receives from client requests, for validation i.e: where to send its OpenIdConnect calls (that will be to our Identity Server).
                   
                    config.Audience = "ClientApi"; //identifying what resource is trying to pass this token for authentication, so we know that its not a random thing that's trying to validate a token, but rather its an API that we already know about.
                });

            services.AddHttpClient(); //this will allow the Client Api to make http requests to our identity server to request for access tokens, which the client api would now use to make requests to the Server Api

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
