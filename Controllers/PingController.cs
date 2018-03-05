using Microsoft.AspNetCore.Mvc;

namespace pk_dotnet.Controllers
{
	[Route("api/[controller]")]
    public class PingController : Controller
    {
        // GET api/ping
        [HttpGet]
        public string Get()
        {
			return "pong";
        }
    }
}
