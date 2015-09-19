using Grind.Core;
using Grind.Core.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Grind.Web.Controllers
{
    [RoutePrefix("news")]
    public class NewsController : ApiController
    {
        private readonly INewsProvider newsProvider = new SentimentalNewsProvider(
            RoleEnvironment.GetConfigurationSettingValue("WebHoseApiKey"),
            RoleEnvironment.GetConfigurationSettingValue("IndicoUrl"),
            RoleEnvironment.GetConfigurationSettingValue("IndicoApiKey"));

        private readonly int keywordCount = int.Parse(RoleEnvironment.GetConfigurationSettingValue("KeywordCount"));
        private readonly int responseCount = int.Parse(RoleEnvironment.GetConfigurationSettingValue("ResponseCount"));

        private const string nextRouteUrlHeader = "X-Next-Url";

        [HttpGet]
        [Route("{performanceScore}/{keyword}")]
        public async Task<HttpResponseMessage> GetNews(int performanceScore, string keyword, HttpRequestMessage request)
        {
            if (request.Headers.Contains(nextRouteUrlHeader))
            {
                string nextUrl = request.Headers.First(h => h.Key == nextRouteUrlHeader).Value.ElementAt(0);

                NewsLookupResult newsResult;
                try
                {
                    if (string.IsNullOrWhiteSpace(nextUrl))
                    {
                        newsResult = await newsProvider.GetNewsFromKeyword(keyword, performanceScore, responseCount, keywordCount);
                    }
                    else
                    {
                        newsResult = await newsProvider.GetNewsFromUrl(nextUrl, performanceScore, keywordCount);
                    }
                }
                catch (ArgumentException e)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(e.Message)
                    };
                }


                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(newsResult, Formatting.Indented))
                };
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("Request must contain header {0}", nextRouteUrlHeader))
                };
            }
        }
    }
}
