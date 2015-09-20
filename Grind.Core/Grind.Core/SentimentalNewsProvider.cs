using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grind.Core.Models;
using Utilities;
using Indico.Net;
using webhose;
using Indico.Net.Models;

namespace Grind.Core
{
    public class SentimentalNewsProvider : INewsProvider
    {
        private readonly WebhoseClient newsClient;
        private readonly TextAnalyser nlpClient;
        private readonly string webHoseToken;

        public SentimentalNewsProvider(string webhoseToken, string indicoUrl, string indicoToken)
        {
            this.webHoseToken = webhoseToken.CheckNotNullOrWhitespace("token");
            this.newsClient = new WebhoseClient(webHoseToken);
            this.nlpClient = new TextAnalyser(indicoUrl.CheckNotNullOrWhitespace("indicoUrl"), 
                indicoToken.CheckNotNullOrWhitespace("indicoToken"));
        }

        public async Task<NewsLookupResult> GetNewsFromKeyword(string keyword,
            int performanceScore, int responseCount, int keywordCount, double namedEntitiesThreshold)
        {
            keyword.CheckNotNullOrWhitespace("keyword");
            (performanceScore >= 0).CheckCondition("performanceScore can't be negative.", "performanceScore");
            (responseCount >= 0).CheckCondition("responseCount can't be negative.", "responseCount");
            (keywordCount >= 0).CheckCondition("keywordCount can't be negative.", "keywordCount");

            var webhoseResponse = await newsClient.SearchAsync(keyword, performanceScore, responseCount, Languages.english);
            return await CreateNewsLookupResult(performanceScore, keywordCount, namedEntitiesThreshold, webhoseResponse);
        }

        public async Task<NewsLookupResult> GetNewsFromUrl(string uri, int performanceScore, int keywordCount, double namedEntitiesThreshold)
        {
            (performanceScore >= 0).CheckCondition("performanceScore can't be negative.", "performanceScore");
            var webhoseResponse = await newsClient.SearchAsync(CleanupNextUri(uri));
            return await CreateNewsLookupResult(performanceScore, keywordCount, namedEntitiesThreshold, webhoseResponse);
        }

        private string CleanupNextUri(string uri)
        {
            if (uri.Contains("?"))
            {
                int start = uri.IndexOf('?') + 1;
                if (start >= uri.Length)
                {
                    throw new ArgumentException("Next URL not in proper format.", uri);
                }
                uri = uri.Substring(start, uri.Length - start);
            }
            return uri.Replace("token=" + webHoseToken, "").Replace("format=html", "");
        }

        private async Task<NewsLookupResult> CreateNewsLookupResult(int performanceScore, int keywordCount, double namedEntitiesThreshold, WebhoseResponse webhoseResponse)
        {
            var newItems = FilterNewsItemsByPerformanceScore(await GetNewsItemsFromWebhoseResponse(webhoseResponse, keywordCount, namedEntitiesThreshold), performanceScore);
            return new NewsLookupResult()
            {
                NextUrl = newItems.Count() > 0 ? webhoseResponse.next : null,
                NewsItems = newItems
            };
        }

        private IEnumerable<NewsItem> FilterNewsItemsByPerformanceScore(IEnumerable<NewsItem> newsItems, int performanceScore)
        {
            return newsItems.Where(news => news.PerformanceScore == performanceScore);
        }

        private async Task<IEnumerable<NewsItem>> GetNewsItemsFromWebhoseResponse(WebhoseResponse webhoseResponse, int keywordCount, double namedEntitiesThreshold)
        {
            Dictionary<string, double>[] keywords = await nlpClient.GetKeywordsAsync(webhoseResponse.posts.Select(p => p.text));
            double[] sentimentAnalysisResults = await nlpClient.AnalyseSentimentAsync(webhoseResponse.posts.Select(p => p.text));
            Dictionary<string, NamedEntity>[] namedEntities = await nlpClient.GetNamedEntities(webhoseResponse.posts.Select(p => p.text), namedEntitiesThreshold);

            var newsItems = new List<NewsItem>();

            for (int i = 0; i < webhoseResponse.posts.Count; ++i)
            {
                var webhosePost = webhoseResponse.posts[i];
                newsItems.Add(new NewsItem()
                {
                    Author = webhosePost.author,
                    KeyWords = keywords[i],
                    PerformanceScore = int.Parse(webhosePost.thread.performanceScore),
                    Published = webhosePost.published,
                    SectionTitle = webhosePost.thread.sectionTitle,
                    SentimentScore = sentimentAnalysisResults[i],
                    SiteSection = webhosePost.thread.siteSection,
                    Title = webhosePost.title,
                    Url = webhosePost.url,
                    Website = webhosePost.thread.site,
                    NamedEntities = namedEntities[i],
                    PoliticalSentiments = await nlpClient.GetPoliticalSentimentsAsync(webhosePost.text)
                });
            }

            return newsItems;
        }
    }
}
