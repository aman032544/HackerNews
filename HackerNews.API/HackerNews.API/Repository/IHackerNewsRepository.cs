using HackerNews.API.ApiOptions;
using HackerNews.API.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace HackerNews.API.Repository
{
    public interface IHackerNewsRepository
    {
        Task<IEnumerable<int>> GetNewStoryIdsAsync();
        Task<Story> GetStoryAsync(int id);
    }
    public class HackerNewsRepository : IHackerNewsRepository
    {
        private readonly HttpClient _httpClient;
        private readonly HackerNewsOptions _hackerNewsOptions;
        public HackerNewsRepository(HttpClient httpClient, IOptions<HackerNewsOptions> options)
        {
            _hackerNewsOptions=options.Value;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<int>> GetNewStoryIdsAsync()
        {
            var response = await _httpClient.GetStringAsync(_hackerNewsOptions.NewStory);
            return JsonSerializer.Deserialize<IEnumerable<int>>(response);
        }

        public async Task<Story> GetStoryAsync(int id)
        {
            var result = await _httpClient.GetStringAsync($"item/{id}.json");
            var doc = JsonDocument.Parse(result);
            var root = doc.RootElement;

            if (!root.TryGetProperty("title", out var titleProp))
                return null;

            var title = titleProp.GetString();
            string? url = null;

            if (root.TryGetProperty("url", out var urlProp))
                url = urlProp.GetString();

            return new Story
            {
                Title = title ?? string.Empty,
                Url = url
            };
        }
    }
}
