using Indico.Net.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;

namespace Indico.Net
{
    public class TextAnalyser
    {
        private const string resultsKey = "results";
        private readonly SimpleHttpRequestMaker requestMaker;
        private readonly string indicoApiKey;

        public TextAnalyser(string apiUrl, string indicoApiKey)
        {
            this.indicoApiKey = indicoApiKey.CheckNotNullOrWhitespace("indicoKey");
            this.requestMaker = new SimpleHttpRequestMaker(apiUrl.CheckNotNullOrWhitespace("url"));
        }

        public Task<double> AnalyseSentimentAsync(string str)
        {
            return MakeDataRequest<double>("sentiment",
                new Dictionary<string, object>() { { "data", str } });
        }

        public Task<double[]> AnalyseSentimentAsync(IEnumerable<string> strs)
        {
            return MakeDataRequest<double[]>(@"sentiment/batch",
                new Dictionary<string, object>() { { "data", strs } });
        }

        public Task<double> AnalyseSentimentHighQualityAsync(string str)
        {
            return MakeDataRequest<double>("sentimenthq",
                new Dictionary<string, object>() { { "data", str } });
        }

        public Task<double[]> AnalyseSentimentHighQualityAsync(IEnumerable<string> strs)
        {
            return MakeDataRequest<double[]>(@"sentimenthq/batch",
                new Dictionary<string, object>() { { "data", strs } });
        }

        public Task<Dictionary<string, double>> GetPoliticalSentimentsAsync(string str)
        {
            return MakeDataRequest<Dictionary<string, double>>("political",
                new Dictionary<string, object>() { { "data", str } });
        }

        public Task<Dictionary<string, double>[]> GetPoliticalSentimentsAsync(IEnumerable<string> strs)
        {
            return MakeDataRequest<Dictionary<string, double>[]>("political/batch",
                new Dictionary<string, object>() { { "data", strs } });
        }

        public Task<Dictionary<string, double>> GetTextTagsAsync(
            string str,
            int? topN = null,
            double? threshold = null,
            bool independent = false)
        {
            return MakeGetTextTagsRequest<Dictionary<string, double>>("texttags", str, topN, threshold, independent);
        }

        public Task<Dictionary<string, double>[]> GetTextTagsAsync(
            IEnumerable<string> strs,
            int? topN = null,
            double? threshold = null,
            bool independent = false)
        {
            return MakeGetTextTagsRequest<Dictionary<string, double>[]>(@"texttags/batch", strs, topN, threshold, independent);
        }

        
        public Task<Dictionary<string, double>> GetKeywordsAsync(
            string str,
            int? topN = null,
            double? threshold = null,
            bool relative = false)
        {
            return MakeGetKeywordsRequest<Dictionary<string, double>>("keywords", str, topN, threshold, relative);
        }

        public Task<Dictionary<string, double>[]> GetKeywordsAsync(
            IEnumerable<string> strs,
            int? topN = null,
            double? threshold = null,
            bool relative = false)
        {
            return MakeGetKeywordsRequest<Dictionary<string, double>[]>(@"keywords/batch", strs, topN, threshold, relative);
        }

        public Task<Dictionary<string, NamedEntity>> GetNamedEntities(string str, double? threshold = null)
        {
            var content = new Dictionary<string, object>()
            {
                { "data", str }
            };

            if (threshold.HasValue)
            {
                content.Add("threshold", threshold);
            }

            return MakeDataRequest<Dictionary<string, NamedEntity>>("namedentities", content);
        }

        public Task<Dictionary<string, NamedEntity>[]> GetNamedEntities(IEnumerable<string> strs, double? threshold = null)
        {
            var content = new Dictionary<string, object>()
            {
                { "data", strs }
            };

            if (threshold.HasValue)
            {
                content.Add("threshold", threshold);
            }

            return MakeDataRequest<Dictionary<string, NamedEntity>[]>(@"namedentities/batch", content);
        }

        private Task<T> MakeGetTextTagsRequest<T>(
            string uri,
            object data,
            int? topN = null,
            double? threshold = null,
            bool independent = false)
        {
            var content = new Dictionary<string, object>()
                {
                    { "data", data }
                };

            if (topN.HasValue)
            {
                content.Add("top_n", topN.Value);
            }

            if (threshold.HasValue)
            {
                content.Add("threshold", threshold.Value);
            }

            if (independent)
            {
                content.Add("independent", independent);
            }

            return MakeDataRequest<T>(uri, content);
        }


        private Task<T> MakeGetKeywordsRequest<T>(
            string uri,
            object data,
            int? topN = null,
            double? threshold = null,
            bool relative = false)
        {
            var content = new Dictionary<string, object>()
                {
                    { "data", data }
                };

            if (topN.HasValue)
            {
                content.Add("top_n", topN.Value);
            }

            if (threshold.HasValue)
            {
                content.Add("threshold", threshold.Value);
            }

            if (relative)
            {
                content.Add("relative", relative);
            }

            return MakeDataRequest<T>(uri, content);
        }

        private async Task<T> MakeDataRequest<T>(string uri, Dictionary<string, object> content)
        {
            var response = await requestMaker.MakeRequestAsync(HttpMethod.Post, GetIndicoUri(uri),
                JsonConvert.SerializeObject(content));

            var results = JsonConvert.DeserializeObject<Dictionary<string, T>>(
                await response.Content.ReadAsStringAsync());

            return results[resultsKey];
        }

        private string GetIndicoUri(string str)
        {
            return string.Format("/{0}?key={1}", str, indicoApiKey);
        }
    }
}
