using System.Threading.Tasks;

namespace pkdotnet
{
	public class ApiClient
	{
		public virtual async Task<string> GetDataFromInternetAsync(string requestUri) {
			System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
			return await client.GetStringAsync(requestUri);
		}
	}
}
