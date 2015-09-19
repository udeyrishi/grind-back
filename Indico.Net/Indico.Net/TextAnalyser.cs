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
        private readonly SimpleHttpRequestMaker requestMaker;
        private readonly string indicoApiKey;

        public TextAnalyser(string apiUrl, string indicoApiKey)
        {
            this.indicoApiKey = indicoApiKey.CheckNotNullOrWhitespace("indicoKey");
            this.requestMaker = new SimpleHttpRequestMaker(apiUrl.CheckNotNullOrWhitespace("url"));
        }

        public async Task<double> AnalyseSentimentAsync(string str)
        {
            var response = await requestMaker.MakeRequestAsync(HttpMethod.Post, GetIndicoUri("sentiment"), 
                GetContentInIndicoJsonFormat(str));

            var results = JsonConvert.DeserializeObject<Dictionary<string, double>>(await response.Content.ReadAsStringAsync());
            return results["results"];
        }

        private string GetIndicoUri(string str)
        {
            return string.Format("/{0}?key={1}", str, indicoApiKey);
        }

        private string GetContentInIndicoJsonFormat(string data)
        {
            return JsonConvert.SerializeObject(new Dictionary<string, string>()
            {
                { "data", data }
            });
        }
    }
}
