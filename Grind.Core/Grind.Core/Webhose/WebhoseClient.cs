using System;
using webhose;
using Grind.Core;
using Utilities;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Grind.Core
{
	public class WebhoseClient
	{
		private string token;

		public WebhoseClient(string token)
		{
			this.token = token;
		
		}

		public async Task<WebhoseResponse> Search(String searchQuery, int performanceScore, string language) {
			WebhoseQuery webhoseQuery = new WebhoseQuery(this.token, searchQuery, performanceScore, language);
			string query = webhoseQuery.ToString();
			var requestMaker = new SimpleHttpRequestMaker ("https://webhose.io/search");
			var response = await requestMaker.MakeRequestAsync (HttpMethod.Get, query); 
			return await ExtractWebhoseResponseAsync (response);
		}

		private async Task<WebhoseResponse> ExtractWebhoseResponseAsync(HttpResponseMessage response)
		{
			string json = await response.Content.ReadAsStringAsync ();
			WebhoseResponse webhoseResponse = JsonConvert.DeserializeObject<WebhoseResponse>(json);
			return webhoseResponse;
		}
	}
}