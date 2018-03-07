using System;
using System.Collections.Generic;
using pkdotnet;

namespace app.tests
{
	public class ApiClientMock:ApiClient
	{
		public readonly Dictionary<string, Func<string>> Operations = new Dictionary<string, Func<string>>();

		public override async System.Threading.Tasks.Task<string> GetDataFromInternetAsync(string requestUri)
		{
			if(Operations.ContainsKey(requestUri)) {
				return Operations[requestUri]();
			}

			throw new Exception("This should be a 404");
		}
	}
}
