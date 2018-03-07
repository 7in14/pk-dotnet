using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace pkdotnet.Controllers
{
	[Route("api/raleigh/[controller]")]
	public class CrimeController : Controller
	{
		private Lazy<ApiClient> _apiClient = new Lazy<ApiClient>();

		public Lazy<ApiClient> Client
		{
			set { _apiClient = value; }
		}

		// GET: /api/file
		[HttpGet]
		public async Task<List<Crime>> Get(string query=null)
		{
			var crimesJson = await _apiClient.Value.GetDataFromInternetAsync("https://data.raleighnc.gov/resource/3bhm-we7a.json");
			var crimes = JsonConvert.DeserializeObject<List<Crime>>(crimesJson);

			if (string.IsNullOrWhiteSpace(query))
			{
				return crimes;
			}

			var filtered = crimes.FindAll((c) => c.LcrDescription.Contains(query));
			return filtered;
		}
	}
}
