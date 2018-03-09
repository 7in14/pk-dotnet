using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pkdotnet;

namespace app.integration.tests.Controllers
{
	[TestClass]
	public class PingControllerTests
	{
		readonly TestServer _server;
		readonly HttpClient _client;

		public PingControllerTests()
		{
			// Arrange
			_server = new TestServer(new WebHostBuilder()
				.UseStartup<Startup>());
			_client = _server.CreateClient();
		}


		[TestMethod]
		public async Task Ping_Should_ReturnPong()
		{
			// Act
			var response = await _client.GetAsync("/api/ping");
			response.EnsureSuccessStatusCode();

			var responseString = await response.Content.ReadAsStringAsync();

			// Assert
			Assert.AreEqual("pong", responseString);
		}
	}
}
