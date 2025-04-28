using HackerNews.Models.Dto;
using HackerNews.Models.Options;
using HackerNews.Repository.Interfaces;
using HackerNews.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Services.Implementations
{
    public class HackerNewsService : IHackerNewsService
    {
        #region Dependency Injection

        private readonly IHackerNewsRepository _repo;
        private readonly IMemoryCache _cache;
        private readonly HackerNewsOptions _hackerNewsOptions;
        public HackerNewsService(IHackerNewsRepository repo, IMemoryCache cache, IOptions<HackerNewsOptions> options)
        {
            _repo = repo;
            _cache = cache;
            _hackerNewsOptions = options.Value;
        }

        #endregion

        #region Main

        public async Task<List<Story>> GetTopStoriesAsync()
        {
            if (_cache.TryGetValue(_hackerNewsOptions.CacheKey, out List<Story>? cachedStories))
                return cachedStories!;

            var ids = await _repo.GetNewStoryIdsAsync();
            var limitedIds = ids.Take(_hackerNewsOptions.TopStoryRange);

            IEnumerable<Task<Story?>> tasks = limitedIds.Select(id => _repo.GetStoryAsync(id));
            var results = await Task.WhenAll(tasks);

            var filtered = results
                .Where(story => story is not null && !string.IsNullOrEmpty(story.Url) && story.Type.ToLower().Equals("story"))
                .ToList()!;

            _cache.Set(_hackerNewsOptions.CacheKey, filtered, TimeSpan.FromMinutes(10));
            return filtered;
        }

        #endregion


    }
}
