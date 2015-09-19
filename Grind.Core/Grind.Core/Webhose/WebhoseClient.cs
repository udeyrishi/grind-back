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
        private readonly WebhoseRequest webhoseRequest;

		public WebhoseClient(string token)
		{
            this.webhoseRequest = new WebhoseRequest(token.CheckNotNullOrWhitespace("token"));
		}

		public Task<WebhoseResponse> Search(string searchQuery, int performanceScore, Languages language)
        {
            return Task.Run(() =>
            {
                return webhoseRequest.getResponse(CreateUrl(searchQuery, performanceScore, language));
            });
		}

        private static string CreateUrl(string query, int performanceScore, Languages language)
        {
            return string.Format("{0}%20performance_score%3A%3E{1}&language={2}",
                query.Replace(" ", "%20"), performanceScore.ToString(), language.ToString());
        }
    }
}