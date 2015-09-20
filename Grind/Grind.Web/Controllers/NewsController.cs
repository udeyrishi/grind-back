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
using System.Web.Http.Cors;

namespace Grind.Web.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("news")]
    public class NewsController : ApiController
    {
        private readonly INewsProvider newsProvider = new SentimentalNewsProvider(
            RoleEnvironment.GetConfigurationSettingValue("WebHoseApiKey"),
            RoleEnvironment.GetConfigurationSettingValue("IndicoUrl"),
            RoleEnvironment.GetConfigurationSettingValue("IndicoApiKey"));

        private readonly int keywordCount = int.Parse(RoleEnvironment.GetConfigurationSettingValue("KeywordCount"));
        private readonly int responseCount = int.Parse(RoleEnvironment.GetConfigurationSettingValue("ResponseCount"));
        private readonly double namedEntitiesThreshold = double.Parse(RoleEnvironment.GetConfigurationSettingValue("NamedEntitiesThreshold"));

        private const string nextRouteUrlHeader = "X-Next-Url";

        [HttpGet]
        [Route("{performanceScore}/{keyword}")]
        public async Task<HttpResponseMessage> GetNews(int performanceScore, string keyword, HttpRequestMessage request)
        {
            NewsLookupResult newsResult;

            if (request.Headers.Contains(nextRouteUrlHeader))
            {
                string nextUrlString = request.Headers.First(h => h.Key == nextRouteUrlHeader).Value.ElementAt(0);
                
                try
                {
                    if (string.IsNullOrWhiteSpace(nextUrlString))
                    {
                        newsResult = await newsProvider.GetNewsFromKeyword(keyword, performanceScore, responseCount, keywordCount, namedEntitiesThreshold);
                    }
                    else
                    {
                        Uri nextUri;
                        if (!Uri.TryCreate(nextUrlString, UriKind.Absolute, out nextUri))
                        {
                            return new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new StringContent(nextRouteUrlHeader + " needs to be a valid URL.")
                            };
                        }

                        newsResult = await newsProvider.GetNewsFromUrl(nextUri, performanceScore, keywordCount, namedEntitiesThreshold);
                    }
                }
                catch (ArgumentException e)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(e.Message)
                    };
                }
            }

            else
            {
                newsResult = await newsProvider.GetNewsFromKeyword(keyword, performanceScore, responseCount, keywordCount, namedEntitiesThreshold);
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(newsResult, Formatting.Indented))
            };
        }
    }
}
