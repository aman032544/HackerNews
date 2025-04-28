using HackerNews.API.Controllers;
using HackerNews.Models.Dto;
using HackerNews.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.API.Test.Controllers
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
        public async Task GetStories_WhenHttpRequestException_Returns503()
        {
            // Arrange
            _mockService.Setup(service => service.GetTopStoriesAsync())
                        .ThrowsAsync(new HttpRequestException("External API failed"));

            // Act
            var result = await _controller.GetStories();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(503, statusCodeResult.StatusCode);
            Assert.Contains("External API error", statusCodeResult.Value.ToString());
        }

        [Fact]
        public async Task GetStories_WhenGeneralException_Returns500()
        {
            // Arrange
            _mockService.Setup(service => service.GetTopStoriesAsync())
                        .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.GetStories();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Contains("Internal server error", statusCodeResult.Value.ToString());
        }
    }
}
