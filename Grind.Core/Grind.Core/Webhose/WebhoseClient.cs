using System.Threading.Tasks;
using Utilities;
using webhose;

namespace Grind.Core
{
    public class WebhoseClient
	{
        private readonly WebhoseRequest webhoseRequest;

		public WebhoseClient(string token)
		{
            this.webhoseRequest = new WebhoseRequest(token.CheckNotNullOrWhitespace("token"));
		}

		public Task<WebhoseResponse> SearchAsync(string searchQuery, int performanceScore, int responseCount, Languages language)
        {
            (performanceScore >= 0).CheckCondition("performanceScore can't be negative.", "performanceScore");
            (responseCount >= 0).CheckCondition("responseCount can't be negative.", "responseCount");
            return Task.Run(() =>
            {
                return webhoseRequest.getResponse(CreateUrl(searchQuery, performanceScore, responseCount, language));
            });
		}

        public Task<WebhoseResponse> SearchAsync(string queryString)
        {
            queryString.CheckNotNullOrWhitespace("queryString");
            return Task.Run(() =>
            {
                return webhoseRequest.getResponse(queryString);
            });
        }

        private static string CreateUrl(string query, int performanceScore, int responseCount, Languages language)
        {
            return string.Format("{0}%20performance_score%3A%3E{1}&language={2}&size={3}",
                query.Replace(" ", "%20"), performanceScore - 1, language.ToString(), responseCount);
        }
    }
}