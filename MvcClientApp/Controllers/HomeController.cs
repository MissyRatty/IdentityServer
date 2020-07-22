using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvcClientApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> SomeRestrictedData()
        {
            //retrieve authentication tokens: id_token, access_token, & refresh_token(this is optional)
            //id_token is your authentication layer : identification of the logged user. This is just to show / bare witness that the client has been authenticated by identity server
            //access_token: your credential to access a resource

            var idToken = await HttpContext.GetTokenAsync("id_token");
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var claims = User.Claims?.ToList();
            var idJwtToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
            var accessJwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            //var refreshJwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);


            var result = await GetDataFromServerApiEndpointUsingAccessToken(accessToken);

            //we want to call the ServerAPI Endpoint in this mvc controller:

            return View();
        }


        public async Task<string> GetDataFromServerApiEndpointUsingAccessToken(string accessToken)
        {
            var apiClient = _httpClientFactory.CreateClient();

            //add authorization header with Bearer Token
            apiClient.SetBearerToken(accessToken);

            var responseFromServerApiDataEndpoint = await apiClient.GetAsync(requestUri: "https://localhost:44333/data");

            var content = await responseFromServerApiDataEndpoint.Content?.ReadAsStringAsync();

            return content;
        }
    }
}
