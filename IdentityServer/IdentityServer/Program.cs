using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //adding a seed user
            using(var scope = host.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                var user = new IdentityUser("rahmat");
                userManager.CreateAsync(user: user, password: "password").GetAwaiter().GetResult();

                //add custom claims for testing

                //first claim to be added to the id_token
                userManager.AddClaimAsync(user, new Claim("my.Claim", "tell.identityeSrverToGiveThisToClient"))
                    .GetAwaiter().GetResult();

                //this one to be added to the access_token
                userManager.AddClaimAsync(user, new Claim("my.api.claim", "big.api.cookie"))
                   .GetAwaiter().GetResult();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
