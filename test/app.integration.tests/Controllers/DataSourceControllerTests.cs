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
		public async Task Get_Should_ReturnDataById_When_Found()
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
			dataSourceMock.VerifyAll();
			Assert.IsTrue(responseString.Contains("http://test-data.com"));
			Assert.IsTrue(responseString.Contains(id));
		}

		[TestMethod]
		public async Task Get_Should_Return404_When_NotFound()
		{
			// Arrange
			const string id = "5a9393c8f055e1b7da739d09";
			dataSourceMock.Setup(ma => ma.GetOne(id))
						  .ReturnsAsync(() => null);

			// Act
			var response = await _client.GetAsync($"/api/dataSource/{id}");
			var responseString = await response.Content.ReadAsStringAsync();

			// Assert
			dataSourceMock.VerifyAll();
			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
			Assert.IsTrue(responseString.Contains($"Data source with id '{id}' not found!"));
		}

		[TestMethod]
		public async Task Delete_Should_Return404_When_NotFound()
		{
			// Arrange
			const string id = "5a9393c8f055e1b7da739d09";
			dataSourceMock.Setup(ma => ma.DeleteOne(id))
						  .ReturnsAsync(() => false);

			// Act
			var response = await _client.DeleteAsync($"/api/dataSource/{id}");
			var responseString = await response.Content.ReadAsStringAsync();

			// Assert
			dataSourceMock.VerifyAll();
			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
			Assert.IsTrue(responseString.Contains($"Could not delete! Data source with id <{id}> not found!"));
		}

		[TestMethod]
		public async Task Delete_Should_Return202_When_Deleted()
		{
			// Arrange
			const string id = "9a9393c8f055e1b7da739d00";
			dataSourceMock.Setup(ma => ma.DeleteOne(id))
			              .ReturnsAsync(() => true);

			// Act
			var response = await _client.DeleteAsync($"/api/dataSource/{id}");
			var responseString = await response.Content.ReadAsStringAsync();

			// Assert
			dataSourceMock.VerifyAll();
			Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
			Assert.IsTrue(responseString.Contains("Deleted"));
		}

		[TestMethod]
		public async Task Add_Should_Return201_When_Created()
		{
			// Arrange
			const string id = "669393c8f055e1b7da739d66";
			const string name = "Unit test new";
			const string url = "https://unit-testing.is/fun";
			var dsJson = $@"{{
				name: ""{name}"",
				url: ""{url}""
			}}";
			dataSourceMock.Setup(ma => ma.AddOne(It.Is<DataSource>((ds=> 
			                                                        ds.Name == name && ds.Url == url))))
						  .ReturnsAsync(() => new DataSource()
						  {
							  Id = new MongoDB.Bson.ObjectId(id),
							  Name = name,
							  Url = url
						  });

			// Act
			var response = await _client.PutAsync("/api/dataSource",new StringContent(dsJson, Encoding.UTF8, "application/json"));
			var responseString = await response.Content.ReadAsStringAsync();

			// Assert
			dataSourceMock.VerifyAll();
			Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
			Assert.IsTrue(responseString.Contains(id));
		}
	}
}
