using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerApi.Controllers
{
    public class DataController : Controller
    {
        [Route("/data")]
        [Authorize] //checking that any request that comes in here has an access token
        public string Index()
        {
            return "Some Test Data From ServerApi";
        }
    }
}
