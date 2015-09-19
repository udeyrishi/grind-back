using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grind.Core.Models
{
    public class NewsLookupResult
    {
        public List<NewsItem> NewsItems { get; set; }
        public string NextUrl { get; set; }
    }
}
