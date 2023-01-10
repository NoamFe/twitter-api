using Microsoft.AspNetCore.Mvc;
using Twitter.Data; 

namespace Twitter.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TweetsController : ControllerBase
    {
         
        private readonly ILogger<TweetsController> _logger;

        public TweetsController(ILogger<TweetsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("totalcount")] 
        public IActionResult GetTotalCount([FromServices] IReadRepository readRepository)
        { 
            return Ok(readRepository.GetTotalTweetCount());
        }

        [HttpGet("{id}")]
        public IActionResult GetTweet([FromServices] IReadRepository readRepository
            , string id)
        {
            return Ok(readRepository.GetTweet(id));
        } 
    }
}