using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pk_dotnet.Controllers;
using pkdotnet;
using pkdotnet.Controllers;

namespace app.tests.Controllers
{
	[TestClass]
	public class CrimeControllerTests
	{
		readonly CrimeController _target;
		readonly ApiClientMock _apiClientMock;
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
			_target = new CrimeController();
			_apiClientMock = new ApiClientMock();
			_target.Client = new System.Lazy<ApiClient>(() => _apiClientMock);
			_apiClientMock.Operations.Add("https://data.raleighnc.gov/resource/3bhm-we7a.json", () => _json);
		}

		[TestMethod]
		public async Task Crime_Should_ReturnCrimes()
		{
			var result = await _target.Get();

			Assert.AreEqual(2, result.Count);
		}
	}
}
