using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ServerApi.Controllers
{
    public class DataController : Controller
    {
        [Route("/data")]
        [Authorize] //checks/validates that any request that comes in here has an access token
        public string Index()
        {
            var claims = User.Claims?.ToList();

            return "Some Test Data From ServerApi";
        }
    }
}
