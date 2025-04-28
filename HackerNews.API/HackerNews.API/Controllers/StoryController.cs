using HackerNews.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IHackerNewsService _service;

        public StoryController(IHackerNewsService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves the top stories from the Hacker News API.
        /// </summary>
        /// <returns>
        /// Returns 200 OK with a list of top stories on success,  
        /// 503 Service Unavailable if an external API error occurs,  
        /// or 500 Internal Server Error for any other exceptions.
        /// </returns>

        [HttpGet("api/Stories")]
        public async Task<IActionResult> GetStories()
        {
            try
            {
                var stories = await _service.GetTopStoriesAsync();
                return Ok(stories);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(503, $"External API error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
