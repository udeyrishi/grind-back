using System;
using webhose;

namespace Grind.Core
{
	public class WebhoseClient
	{
		private string token;

		public WebhoseClient(string token)
		{
			this.token = token;
		
		}

		public WebhoseResponse search(String searchQuery) throws IOException {
			WebhoseQuery query = new WebhoseQuery(this.token, searchQuery, 4, "english");
			return search(query);
		}
			
		public WebhoseResponse search(WebhoseQuery query) throws IOException {
			string url = query.ToString();

			return 
		}

		public async Task<string> MakeSearchRequest(string url)
		{
			var requestMaker = new SimpleHttpRequestMaker (url);
			//TODO GET
			HttpResponseMessage responseTask = await requestMaker.MakeRequestAsync (HttpMethod.Get, url);
			return await responseTask.Content.ReadAsStringAsync ();
		}

		public async Task<bool> Process(string url)
		{
			string response = await MakeSearchRequest (url);

			return true;
		}

	}
}

