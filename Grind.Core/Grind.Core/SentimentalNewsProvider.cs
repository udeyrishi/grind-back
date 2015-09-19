using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grind.Core.Models;
using Utilities;
using Indico.Net;
using webhose;

namespace Grind.Core
{
    public class SentimentalNewsProvider : INewsProvider
    {
        private readonly WebhoseClient newsClient;
        private readonly TextAnalyser nlpClient;

        public SentimentalNewsProvider(string webhoseToken, string indicoUrl, string indicoToken)
        {
            this.newsClient = new WebhoseClient(webhoseToken.CheckNotNullOrWhitespace("token"));
            this.nlpClient = new TextAnalyser(indicoUrl.CheckNotNullOrWhitespace("indicoUrl"), 
                indicoToken.CheckNotNullOrWhitespace("indicoToken"));
        }

        public async Task<NewsLookupResult> GetNewsFromKeyword(string keyword,
            int performanceScore, int responseCount, int keywordCount)
        {
            keyword.CheckNotNullOrWhitespace("keyword");
            (performanceScore >= 0).CheckCondition("performanceScore can't be negative.", "performanceScore");
            (responseCount >= 0).CheckCondition("responseCount can't be negative.", "responseCount");
            (keywordCount >= 0).CheckCondition("keywordCount can't be negative.", "keywordCount");

            var webhoseResponse = await newsClient.SearchAsync(keyword, performanceScore, responseCount, Languages.english);
            return await CreateNewsLookupResult(performanceScore, keywordCount, webhoseResponse);
        }

        public async Task<NewsLookupResult> GetNewsFromUrl(string uri, int performanceScore, int keywordCount)
        {
            (performanceScore >= 0).CheckCondition("performanceScore can't be negative.", "performanceScore");
            if (uri.Contains("?"))
            {
                int start = uri.IndexOf('?') + 1;
                if (start >= uri.Length)
                {
                    throw new ArgumentException("Next URL not in proper format.", uri);
                }
                uri = uri.Substring(start, uri.Length - start);
            }
            var webhoseResponse = await newsClient.SearchAsync(uri);
            return await CreateNewsLookupResult(performanceScore, keywordCount, webhoseResponse);
        }

        private async Task<NewsLookupResult> CreateNewsLookupResult(int performanceScore, int keywordCount, WebhoseResponse webhoseResponse)
        {
            var newItems = FilterNewsItemsByPerformanceScore(await GetNewsItemsFromWebhoseResponse(webhoseResponse, keywordCount), performanceScore);
            return new NewsLookupResult()
            {
                NextUrl = newItems.Count > 0 ? webhoseResponse.next : null,
                NewsItems = newItems
            };
        }

        private List<NewsItem> FilterNewsItemsByPerformanceScore(List<NewsItem> newsItems, int performanceScore)
        {
            return newsItems.Where(news => news.PerformanceScore == performanceScore).ToList();
        }

        private async Task<List<NewsItem>> GetNewsItemsFromWebhoseResponse(WebhoseResponse webhoseResponse, int keywordCount)
        {
            var newsItems = new List<NewsItem>();
            foreach (var webhosePost in webhoseResponse.posts)
            {
                newsItems.Add(
                    new NewsItem()
                    {
                        Author = webhosePost.author,
                        KeyWords = await nlpClient.GetKeywordsAsync(webhosePost.text, keywordCount),
                        PerformanceScore = int.Parse(webhosePost.thread.performanceScore),
                        Published = webhosePost.published,
                        SectionTitle = webhosePost.thread.sectionTitle,
                        SentimentScore = await nlpClient.AnalyseSentimentAsync(webhosePost.text),
                        SiteSection = webhosePost.thread.siteSection,
                        Title = webhosePost.title,
                        Url = webhosePost.url,
                        Website = webhosePost.thread.site
                    });
            }
            return newsItems;
        }
    }
}
