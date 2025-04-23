using HackerNews.API.Models;

namespace HackerNews.API.Services
{
    public interface IHackerNewsService
    {
        Task<List<Story>> GetTopStoriesAsync();
    }
}
