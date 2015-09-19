using Grind.Core;
using Grind.Core.Models;
using Newtonsoft.Json;
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
        private readonly INewsProvider newsProvider = new SentimentalNewsProvider(); 
        private const string nextRouteUrlHeader = "X-Next-Url";

        [HttpGet]
        [Route("{performanceScore}/{keyword}")]
        public async Task<HttpResponseMessage> GetNews(int performanceScore, string keyword, HttpRequestMessage request)
        {
            if (request.Headers.Contains(nextRouteUrlHeader))
            {
                string nextUrl = request.Headers.First(h => h.Key == nextRouteUrlHeader).Value.ElementAt(0);

                NewsLookupResult newsResult;
                if (string.IsNullOrWhiteSpace(nextUrl))
                {
                    newsResult = await newsProvider.GetNewsFromKeyword(keyword, performanceScore);
                }
                else
                {
                    newsResult = await newsProvider.GetNewsFromUrl(nextUrl, performanceScore);
                }

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(newsResult))
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
