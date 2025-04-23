using HackerNews.API.Services;
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

        [HttpGet]
        public async Task<IActionResult> GetStories()
        {
            try
            {
                var result = await _service.GetTopStoriesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Unable to fetch stories right now.");
            }
        }
    }
}
