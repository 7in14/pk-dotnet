using System;
using Newtonsoft.Json;

namespace pkdotnet
{
	public class Crime
	{
		[JsonProperty("district")]
		public string District { get; set; }

		[JsonProperty("inc_datetime")]
		public DateTime IncidentDateTime { get; set; }

		[JsonProperty("inc_no")]
		public string IncidentNumber { get; set; }

		[JsonProperty("lcr")]
		public string Lcr { get; set; }

		[JsonProperty("lcr_desc")]
		public string LcrDescription { get; set; }
	}
}