using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MvcClientApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config =>
            {
                config.DefaultScheme = "Cookie";
                config.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookie")
                .AddOpenIdConnect("oidc", config => //since its openIdconnect, this middleware will know how to get to the discovery document and know how to communicate through the background process
                {
                    config.Authority = "https://localhost:44338/";
                    config.ClientId = "clientMvc_id";
                    config.ClientSecret = "clientMvc_secret";
                    config.SaveTokens = true;                 //save tokens in our browser cookies
                    config.ResponseType = "code";


                    //configure user claims to map to my cookie correctly
                    //this is a list of action values you can use to select vals from the json user data and create/delete my desired claims

                    //delete
                    config.ClaimActions.DeleteClaim("amr");
                    config.ClaimActions.DeleteClaim("s_hash");

                    //create
                    config.ClaimActions.MapUniqueJsonKey("RawClaim.MyClaim", "my.Claim");
                    

                    //this will do a round trip just to get the additional user claims.
                    //so its either this or you end up with a big id_token

                    //two trips to load claims in to the cookie, but the id_token is snmaller
                    config.GetClaimsFromUserInfoEndpoint = true;  //this tells it to go to the user info endpoint to retrieve additional claims after it has created the identity from the token endpoint


                    config.Scope.Clear();

                    //configure my own defined scope (this was added in the Identity Server)
                    //tell the client app to request access to these scopes
                    config.Scope.Add("openid");
                    config.Scope.Add("my.OwnDefinedScope");
                    config.Scope.Add("ServerApi");
                    config.Scope.Add("ClientApi");
                });

            //added this line so we can make http client calls to other API end points
            services.AddHttpClient();

            services.AddControllersWithViews();
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
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
