using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pkdotnet;

namespace app.integration.tests
{
	[TestClass]
	public class LoggerMiddlewareTests
	{
		readonly TestServer _server;
		readonly HttpClient _client;

		public LoggerMiddlewareTests()
		{
			// Arrange
			_server = new TestServer(new WebHostBuilder()
									 .UseStartup<Startup>());
			_client = _server.CreateClient();
		}

		[TestMethod]
		public async Task Should_Handle_404()
		{
			// Act
			var response = await _client.GetAsync("/api/does-not-exist");
			var statusCode = response.StatusCode;

			// Assert
			Assert.AreEqual(HttpStatusCode.NotFound, statusCode);
		}
	}
}
