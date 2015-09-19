using Grind.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grind.Core
{
    public interface INewsProvider
    {
        Task<NewsLookupResult> GetNewsFromKeyword(string keyword, int performanceScore);
        Task<NewsLookupResult> GetNewsFromUrl(string uri, int performanceScore);
    }
}
