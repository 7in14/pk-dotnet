using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pkdotnet;

namespace app.integration.tests.Controllers
{
	[TestClass]
	public class CrimeControllerTests
	{
		readonly TestServer _server;
		readonly HttpClient _client;
		readonly string _json = @"[{
    ""district"": ""SOUTHEAST"",
    ""inc_datetime"": ""2018-02-02T00:00:00.000"",
    ""lcr"": ""71A"",
    ""lcr_desc"": ""Traffic/DWI (Driving While Impaired)"",
}, {
    ""district"": ""SOUTHEAST"",
    ""inc_datetime"": ""2018-02-02T00:39:00.000"",
    ""lcr"": ""54D"",
    ""lcr_desc"": ""Drug Violation/Misdemeanor"",
}]";

		public CrimeControllerTests()
		{
			// Arrange
			_server = new TestServer(new WebHostBuilder()
				.UseStartup<Startup>());
			_client = _server.CreateClient();
		}

		[TestMethod]
		public async Task Should_Return_CrimesFromRaleighOpenData()
		{
			using (var httpTest = new Flurl.Http.Testing.HttpTest())
			{
				// Arrange
				httpTest.RespondWith(_json);

				// Act
				var response = await _client.GetAsync("/api/raleigh/crime");
				response.EnsureSuccessStatusCode();
				var responseString = await response.Content.ReadAsStringAsync();

				// Assert
				Assert.IsTrue(responseString.Contains("Traffic/DWI (Driving While Impaired)"));
				httpTest.ShouldHaveCalled("https://data.raleighnc.gov/resource/3bhm-we7a.json");
			}
		}

		[TestMethod]
		public async Task Should_Return_CrimesFromRaleighOpenData_When_Filtered()
		{
			using (var httpTest = new Flurl.Http.Testing.HttpTest())
			{
				// Arrange
				httpTest.RespondWith(_json);

				// Act
				var response = await _client.GetAsync("/api/raleigh/crime?query=Drug");
				response.EnsureSuccessStatusCode();
				var responseString = await response.Content.ReadAsStringAsync();

				// Assert
				Assert.IsFalse(responseString.Contains("Traffic/DWI (Driving While Impaired)"), "Filtered list should not have Traffic/DWI");
				Assert.IsTrue(responseString.Contains("Drug Violation/Misdemeanor"), "Fitered list should only have Drug crimes");
				httpTest.ShouldHaveCalled("https://data.raleighnc.gov/resource/3bhm-we7a.json");
			}
		}
	}
}
