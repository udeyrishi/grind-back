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
            return Task.Run(() =>
            {
                return webhoseRequest.getResponse(CreateUrl(searchQuery, performanceScore, responseCount, language));
            });
		}

        private static string CreateUrl(string query, int performanceScore, int responseCount, Languages language)
        {
            return string.Format("{0}%20performance_score%3A%3E{1}&language={2}&size={3}",
                query.Replace(" ", "%20"), performanceScore, language.ToString(), responseCount);
        }
    }
}