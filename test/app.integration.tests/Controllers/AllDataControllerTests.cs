using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using pkdotnet;
using pkdotnet.DataAccess;
using pkdotnet.Models;

namespace app.integration.tests.Controllers
{
	[TestClass]
	public class AllDataControllerTests
	{
		static readonly Mock<IConfiguration> configMock = new Mock<IConfiguration>();
		static readonly Mock<MongoAccess> dataSourceMock = new Mock<MongoAccess>(configMock.Object);

		public class TestStartup : Startup
		{
			public TestStartup(IConfiguration configuration) : base(configuration)
			{
			}

			public override void ConfigureServices(IServiceCollection services)
			{
				base.ConfigureServices(services);
				ServiceCollectionDescriptorExtensions.Replace(services, new ServiceDescriptor(typeof(MongoAccess), dataSourceMock.Object));
			}
		}

		TestServer _server;
		HttpClient _client;
		List<DataSource> _dataSources = new List<DataSource>
		{
			new DataSource{
				Id = new MongoDB.Bson.ObjectId("5a9393c8f055e1b7da739d01"),
				Name = "mock server",
				Url = "http://mock-server/api/data"
			}
		};

		[TestInitialize]
		public void Initialize()
		{
			// Arrange
			dataSourceMock.Reset();
			_server = new TestServer(new WebHostBuilder()
				.UseStartup<TestStartup>());
			_client = _server.CreateClient();
		}

		[TestMethod]
		public async Task Get_Should_ReturnAllData_When_ApiReturnsOK()
		{
			using (var httpTest = new Flurl.Http.Testing.HttpTest())
			{
				// Arrange
				httpTest.RespondWithJson(new { msg = "hello from the mock" }, 200);
				dataSourceMock.Setup(ma => ma.GetAll())
				  .ReturnsAsync(() => _dataSources);

				// Act
				var response = await _client.GetAsync("/api/allData");
				response.EnsureSuccessStatusCode();
				var responseString = await response.Content.ReadAsStringAsync();

				// Assert
				dataSourceMock.VerifyAll();
				Assert.IsTrue(responseString.Contains("hello from the mock"), $"Response string {responseString} should contain the url of mock server");
			}
		}

		[TestMethod]
		public async Task Get_Should_Error_When_ApiTimeouts()
		{
			using (var httpTest = new Flurl.Http.Testing.HttpTest())
			{
				// Arrange
				httpTest.SimulateTimeout();
				dataSourceMock.Setup(ma => ma.GetAll())
				  .ReturnsAsync(() => _dataSources);

				// Act
				var response = await _client.GetAsync("/api/allData");
				response.EnsureSuccessStatusCode();
				var responseString = await response.Content.ReadAsStringAsync();

				// Assert
				dataSourceMock.VerifyAll();
				Assert.IsTrue(responseString.Contains("GET http://mock-server/api/data timed out."), $"Response string {responseString} should contain the url of mock server");
			}
		}
	}
}
