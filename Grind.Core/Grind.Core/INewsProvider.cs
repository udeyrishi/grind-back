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
        Task<NewsLookupResult> GetNewsFromKeyword(string keyword, int performanceScore, int responseCount, int keywordCount, double namedEntitiesThreshold);
        Task<NewsLookupResult> GetNewsFromUrl(Uri uri, int performanceScore, int keywordCount, double namedEntitiesThreshold);
    }
}
