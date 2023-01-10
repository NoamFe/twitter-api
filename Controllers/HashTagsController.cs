using Microsoft.AspNetCore.Mvc;
using Twitter.Data; 

namespace Twitter.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HashTagsController : ControllerBase
    {
         
        private readonly ILogger<HashTagsController> _logger;

        public HashTagsController(ILogger<HashTagsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("top/{take:Int}")]
        public IActionResult GetTopHashTags(
            [FromServices] IReadRepository readRepository,
            int take)
        {
            return Ok(readRepository.GetTopHashTags(take));

        }

        [HttpGet("{tag}/tweet")] 
        public IActionResult GetTweetsByHashTag(
            [FromServices] IReadRepository readRepository,
            string tag)
        { 
            return Ok(readRepository.GetTweetsByHashtag(tag));
        }
    }
}