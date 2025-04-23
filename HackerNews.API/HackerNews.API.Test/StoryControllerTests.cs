using HackerNews.API.Controllers;
using HackerNews.API.Models;
using HackerNews.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace HackerNews.API.Test
{
    public class StoryControllerTests
    {
        private readonly Mock<IHackerNewsService> _mockService;
        private readonly StoryController _controller;

        public StoryControllerTests()
        {
            // Arrange: Initialize the mock service and controller
            _mockService = new Mock<IHackerNewsService>();
            _controller = new StoryController(_mockService.Object);
        }
        [Fact]
        public async Task GetStories_ReturnsOkResult_WhenServiceReturnsData()
        {
            // Arrange
            Task<List<Story>> expectedStories =
                Task.Run(() =>
                {
                    var storylist = new List<Story>();
                    storylist.Add(new Story { Title = "Story1" });
                    storylist.Add(new Story { Title = "Story2" });
                    storylist.Add(new Story { Title = "Story3" });
                    return storylist;
                });
                
             _mockService.Setup(service => service.GetTopStoriesAsync())
                .Returns(expectedStories);

            // Act
            var result = await _controller.GetStories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Story>>(okResult.Value);
            Assert.Equal(expectedStories.Result.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetStories_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _mockService.Setup(service => service.GetTopStoriesAsync())
                .ThrowsAsync(new Exception("Service failure"));

            // Act
            var result = await _controller.GetStories();

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, errorResult.StatusCode);
            Assert.Equal("Unable to fetch stories right now.", errorResult.Value);
        }
    }
}