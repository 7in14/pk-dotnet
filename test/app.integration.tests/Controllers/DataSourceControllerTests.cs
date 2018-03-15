using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pkdotnet.DataAccess;
using Moq;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using pkdotnet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using pkdotnet.Models;

namespace app.integration.tests.Controllers
{
	[TestClass]
	public class DataSourceControllerTests
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

		private TestServer _server;
		private HttpClient _client;

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
		public async Task DataSourceById_Should_Return404_When_NotFound()
		{
			// Arrange
			const string id = "5a9393c8f055e1b7da739d09";
			dataSourceMock.Setup(ma => ma.GetOne(id))
			  .ReturnsAsync(() => new DataSource()
			  {
				  Id = new MongoDB.Bson.ObjectId(id),
				  Name = "test data",
				  Url = "http://test-data.com"
			  });

			// Act
			var response = await _client.GetAsync($"/api/dataSource/{id}");
			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();

			// Assert
			Assert.IsTrue(responseString.Contains("http://test-data.com"));
			Assert.IsTrue(responseString.Contains(id));
		}
	}
}
