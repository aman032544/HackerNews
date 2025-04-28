using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Models.Options
{
    public class HackerNewsOptions
    {
        public string Url { get; set; }
        public string NewStory { get; set; }
        public string CacheKey { get; set; }
        public int TopStoryRange { get; set; }

    }
}
