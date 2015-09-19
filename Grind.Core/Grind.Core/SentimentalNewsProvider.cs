using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grind.Core.Models;

namespace Grind.Core
{
    public class SentimentalNewsProvider : INewsProvider
    {
        public Task<NewsLookupResult> GetNewsFromKeyword(string keyword, int performanceScore)
        {
            throw new NotImplementedException();
        }

        public Task<NewsLookupResult> GetNewsFromUrl(string uri, int performanceScore)
        {
            throw new NotImplementedException();
        }
    }
}
