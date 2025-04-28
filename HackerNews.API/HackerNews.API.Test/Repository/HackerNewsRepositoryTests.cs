using Xunit;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using HackerNews.Models.Dto;
using HackerNews.Models.Options;
using HackerNews.Repository.Implementation;

public class HackerNewsRepositoryTests
{
    private readonly Mock<HttpMessageHandler> _handlerMock;
    private readonly HttpClient _httpClient;
    private readonly HackerNewsOptions _options;
    private readonly HackerNewsRepository _repository;

    public HackerNewsRepositoryTests()
    {
        _handlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new System.Uri("https://hacker-news.firebaseio.com/v0/")
        };

        _options = new HackerNewsOptions
        {
            NewStory = "newstories.json"
        };

        var optionsMock = Mock.Of<IOptions<HackerNewsOptions>>(opt => opt.Value == _options);

        _repository = new HackerNewsRepository(_httpClient, optionsMock);
    }

    [Fact]
    public async Task GetNewStoryIdsAsync_ReturnsListOfIds()
    {
        // Arrange
        var expectedIds = new List<int> { 1, 2, 3 };
        var json = JsonConvert.SerializeObject(expectedIds);

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().EndsWith(_options.NewStory)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json)
            });

        // Act
        var result = await _repository.GetNewStoryIdsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedIds, result);
    }

    [Fact]
    public async Task GetStoryAsync_ReturnsStory()
    {
        // Arrange
        var story = new Story
        {
            Id = 1,
            Title = "Sample Story",
            Url = "https://example.com",
            Type = "story"
        };
        var json = JsonConvert.SerializeObject(story);

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains("item/1.json")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json)
            });

        // Act
        var result = await _repository.GetStoryAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(story.Id, result.Id);
        Assert.Equal(story.Title, result.Title);
    }
}
