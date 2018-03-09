using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Flurl.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace pkdotnet.Controllers
{
	[Route("api/raleigh/[controller]")]
	public class CrimeController : Controller
	{
		// GET: /api/raleigh/crime
		[HttpGet]
		public async Task<List<Crime>> Get(string query=null)
		{
			var crimes = await "https://data.raleighnc.gov/resource/3bhm-we7a.json".GetJsonAsync<List<Crime>>();

			if (string.IsNullOrWhiteSpace(query))
			{
				return crimes;
			}

			var filtered = crimes.FindAll((c) => c.LcrDescription.Contains(query));
			return filtered;
		}
	}
}
