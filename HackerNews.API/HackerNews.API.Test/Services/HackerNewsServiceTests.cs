using Moq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using HackerNews.Models.Dto;
using HackerNews.Models.Options;
using HackerNews.Repository.Interfaces;
using HackerNews.Services.Implementations;

public class HackerNewsServiceTests
{
    private readonly Mock<IHackerNewsRepository> _repoMock;
    private readonly IMemoryCache _cache;
    private readonly HackerNewsOptions _options;
    private readonly HackerNewsService _service;

    public HackerNewsServiceTests()
    {
        _repoMock = new Mock<IHackerNewsRepository>();
        _cache = new MemoryCache(new MemoryCacheOptions());
        _options = new HackerNewsOptions
        {
            CacheKey = "TopStoriesCacheKey",
            TopStoryRange = 3
        };

        var optionsMock = Mock.Of<IOptions<HackerNewsOptions>>(opt => opt.Value == _options);
        _service = new HackerNewsService(_repoMock.Object, _cache, optionsMock);
    }

    [Fact]
    public async Task GetTopStoriesAsync_ReturnsStories_WhenNotInCache()
    {
        // Arrange
        var storyIds = new List<int> { 1, 2, 3, 4 };
        _repoMock.Setup(r => r.GetNewStoryIdsAsync())
            .ReturnsAsync(storyIds);

        _repoMock.Setup(r => r.GetStoryAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => new Story
            {
                Id = id,
                Title = $"Story {id}",
                Url = $"https://example.com/story{id}",
                Type = "story"
            });

        // Act
        var stories = await _service.GetTopStoriesAsync();

        // Assert
        Assert.NotNull(stories);
        Assert.Equal(_options.TopStoryRange, stories.Count);
        Assert.All(stories, s => Assert.False(string.IsNullOrEmpty(s.Url)));
    }

    [Fact]
    public async Task GetTopStoriesAsync_ReturnsStories_FromCache()
    {
        // Arrange
        var cachedStories = new List<Story>
        {
            new Story { Id = 1, Title = "Cached Story", Url = "https://cached.com/story1", Type = "story" }
        };
        _cache.Set(_options.CacheKey, cachedStories, TimeSpan.FromMinutes(10));

        // Act
        var stories = await _service.GetTopStoriesAsync();

        // Assert
        Assert.NotNull(stories);
        Assert.Single(stories);
        Assert.Equal("Cached Story", stories[0].Title);

        // Verify that repository is NOT called when data is in cache
        _repoMock.Verify(r => r.GetNewStoryIdsAsync(), Times.Never);
    }
}
