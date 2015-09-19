using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grind.Core.Models
{
    public class NewsItem
    {
        public string Author { get; set; }

        public string Published { get; set; }

        public double SentimentScore { get; set; }

        public Dictionary<string, double> KeyWords { get; set; }

        public string Url { get; set; }

        public string Website { get; set; }

        public int PerformanceScore { get; set; }
        
        public string SectionTitle { get; set; }
        
        public string SiteSection { get; set; }
        
        public string Title { get; set; }
    }
}
