using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Newtonsoft.Json;

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
            return MakeDataRequest<double>("sentiment", str);
        }

        public Task<double[]> AnalyseSentimentAsync(IEnumerable<string> strs)
        {
            return MakeDataRequest<double[]>(@"sentiment/batch", strs);
        }

        public Task<double> AnalyseSentimentHighQualityAsync(string str)
        {
            return MakeDataRequest<double>("sentimenthq", str);
        }

        public Task<double[]> AnalyseSentimentHighQualityAsync(IEnumerable<string> strs)
        {
            return MakeDataRequest<double[]>(@"sentimenthq/batch", strs);
        }

        public Task<Dictionary<string, double>> GetTextTagsAsync(string str)
        {
            return MakeDataRequest<Dictionary<string, double>>("texttags", str);
        }

        public Task<Dictionary<string, double>[]> GetTextTagsAsync(IEnumerable<string> strs)
        {
            return MakeDataRequest<Dictionary<string, double>[]>(@"texttags/batch", strs);
        }

        private async Task<T> MakeDataRequest<T>(string uri, object content)
        {
            var response = await requestMaker.MakeRequestAsync(HttpMethod.Post, GetIndicoUri(uri),
                GetContentInIndicoJsonFormat(content));

            var results = JsonConvert.DeserializeObject<Dictionary<string, T>>(
                await response.Content.ReadAsStringAsync());

            return results[resultsKey];
        }

        private string GetIndicoUri(string str)
        {
            return string.Format("/{0}?key={1}", str, indicoApiKey);
        }

        private string GetContentInIndicoJsonFormat(object data)
        {
            return JsonConvert.SerializeObject(new Dictionary<string, object>()
            {
                { "data", data }
            });
        }
    }
}
