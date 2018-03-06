using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace pkdotnet
{
	[Route("api/raleigh/[controller]")]
	public class CrimeController : Controller
	{
		// GET: /api/file
		[HttpGet]
		public async Task<string> Get()
		{
			//const crimesResponse = await Got('https://data.raleighnc.gov/resource/3bhm-we7a.json', {
			System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
			var crimesJson = await client.GetStringAsync("https://data.raleighnc.gov/resource/3bhm-we7a.json");
			var crimes = JsonConvert.DeserializeObject(crimesJson);

			return crimesJson;
		}
	}
}
