using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pkdotnet;

namespace app.integration.tests.Middleware
{
	[TestClass]
	public class ErrorHandlingMiddlewareTests
	{
		readonly TestServer _server;
		readonly HttpClient _client;

		public ErrorHandlingMiddlewareTests()
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
				httpTest.RespondWith("Some Error", status: 500);

				// Act
				var response = await _client.GetAsync("/api/raleigh/crime");
				var responseString = await response.Content.ReadAsStringAsync();

				// Assert
				Assert.IsTrue(responseString.Contains("Some Error"));
				Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
				httpTest.ShouldHaveCalled("https://data.raleighnc.gov/resource/3bhm-we7a.json");
			}
		}
	}
}
