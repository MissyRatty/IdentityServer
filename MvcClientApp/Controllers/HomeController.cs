using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
            //refresh_token: is not a jwt, its a random encrypted number that is stored by the identity server and can be retrieved and can be exchanged for an access_token
            
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


        private async Task<string> GetDataFromServerApiEndpointUsingAccessToken(string accessToken)
        {
            var apiClient = _httpClientFactory.CreateClient();

            //add authorization header with Bearer Token
            apiClient.SetBearerToken(accessToken);

            var responseFromServerApiDataEndpoint = await apiClient.GetAsync(requestUri: "https://localhost:44333/data");

            var content = await responseFromServerApiDataEndpoint.Content?.ReadAsStringAsync();

            await RefreshAccessToken();

            return content;
        }

        private async Task RefreshAccessToken()
        {
            var serverClient = _httpClientFactory.CreateClient();

            //this will get the well known endpoint that contains all the info (endpoints) we need to communicate/know with/about our identity server
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44338/"); // so we can get the TokenEndpoint

            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            var refreshTokenClient = _httpClientFactory.CreateClient();

            var idTokenOld = await HttpContext.GetTokenAsync("id_token");
            var accessTokenOld = await HttpContext.GetTokenAsync("access_token");
            var refreshTokenOld = await HttpContext.GetTokenAsync("refresh_token");

            var tokenResponse = await refreshTokenClient.RequestRefreshTokenAsync(new RefreshTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                RefreshToken = refreshToken,
                ClientId = "clientMvc_id",
                ClientSecret = "clientMvc_secret"
            });


            var isAccessTokenNew = !accessTokenOld.Equals(tokenResponse.AccessToken);
            var isIdTokenNew = !idTokenOld.Equals(tokenResponse.IdentityToken);
            var isRefreshTokenNew = !refreshTokenOld.Equals(tokenResponse.RefreshToken);

            var authInfo = await HttpContext.AuthenticateAsync("MvcClientCookie");

            authInfo.Properties.UpdateTokenValue("access_token", tokenResponse.AccessToken);
            authInfo.Properties.UpdateTokenValue("id_token", tokenResponse.IdentityToken); // this may not be necessary
            authInfo.Properties.UpdateTokenValue("refresh_token", tokenResponse.RefreshToken);

            await HttpContext.SignInAsync("MvcClientCookie", authInfo.Principal, authInfo.Properties);

        }
    }
}
