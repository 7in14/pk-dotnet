using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pk_dotnet;
using pk_dotnet.Controllers;

namespace app.integration.tests.Controllers
{
	[TestClass]
	public class FileControllerTests
	{
		readonly TestServer _server;
		readonly HttpClient _client;

		public FileControllerTests()
		{
			/// Users / piotrkarpala / projects / 7in14 / pk - dotnet / app
			string s = Assembly.GetExecutingAssembly().CodeBase;
			s = (new Uri(s)).AbsolutePath;
			var contentRoot = Path.Combine(Path.GetDirectoryName(s), "../../../../../src/app");

			// Arrange
			_server = new TestServer(new WebHostBuilder()
				.UseStartup<Startup>().UseContentRoot(contentRoot));
			_client = _server.CreateClient();
		}


		[TestMethod]
		public async Task File_Should_ReturnReadme()
		{
			// Act
			var response = await _client.GetAsync("/api/file");
			response.EnsureSuccessStatusCode();

			var responseString = await response.Content.ReadAsStringAsync();

			// Assert
			Assert.IsTrue(responseString.Contains("Sample API created with dotnet core"));
		}
	}
}
