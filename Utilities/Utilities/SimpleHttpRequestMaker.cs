using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Utilities
{
    public class SimpleHttpRequestMaker
    {
        private readonly HttpClient httpClient;

        public SimpleHttpRequestMaker(string baseUri = null)
        {
            this.httpClient = new HttpClient();
            if (!string.IsNullOrWhiteSpace(baseUri))
            {
                httpClient.BaseAddress = new Uri(baseUri);
            }
        }

        public Task<HttpResponseMessage> MakeRequestAsync(
            HttpMethod method, 
            string uri, 
            string content = null, 
            IDictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage(method, uri);

            if (headers != null)
            {
                foreach(var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            
            if (!string.IsNullOrWhiteSpace(content))
            {
                request.Content = new StringContent(content);
            }

            return httpClient.SendAsync(request);
        }
    }
}
