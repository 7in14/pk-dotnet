using System.Collections.Generic;
using System.Net.Http;
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
	public class DataSourcesControllerTests
	{
		private static readonly Mock<IConfiguration> configMock = new Mock<IConfiguration>();
		private static readonly Mock<MongoAccess> dataSourceMock = new Mock<MongoAccess>(configMock.Object);

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

		private readonly TestServer _server;
		private readonly HttpClient _client;

		public DataSourcesControllerTests()
		{
			// Arrange
			_server = new TestServer(new WebHostBuilder()
				.UseStartup<TestStartup>());
			_client = _server.CreateClient();
		}

		[TestMethod]
		public async Task DataSources_Should_ReturnAList()
		{
			// Arrange
			dataSourceMock.Setup(ma => ma.GetAll())
			  .ReturnsAsync(() => new List<DataSource>(){
								new DataSource() {
										Id = new MongoDB.Bson.ObjectId(),
										Name = "test data",
										Url = "http://test-data.com" }
			});

			// Act
			var response = await _client.GetAsync("/api/dataSources");
			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();

			// Assert
			Assert.IsTrue(responseString.Contains("http://test-data.com"));
		}
	}
}
