using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Route("/GetDataFromServerApi")]
        public async Task<IActionResult> Index()
        {
            var serverClient = _httpClientFactory.CreateClient();

            //this will get the well known endpoint that contains all the info (endpoints) we need to communicate/know with/about our identity server
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44338/");

            //request access_token
            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "clientApi_id",
                ClientSecret = "clientApi_secret",
                Scope = "ServerApi"
            });

            //request for some data from the ServerApi Data Endpoint
            var apiClient = _httpClientFactory.CreateClient();

            //add authorization header with Bearer Token
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var responseFromServerApiDataEndpoint = await apiClient.GetAsync(requestUri: "https://localhost:44333/data");

            if (responseFromServerApiDataEndpoint.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return Json(responseFromServerApiDataEndpoint);
            }

            var content = await responseFromServerApiDataEndpoint.Content?.ReadAsStringAsync();

            return Json(
                new
                {
                    access_token = tokenResponse.AccessToken,
                    message = content
                });
        }
    }
}
