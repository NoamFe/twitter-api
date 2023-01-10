using Microsoft.AspNetCore.Mvc;
using Twitter.Data; 

namespace Twitter.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthorController : ControllerBase
    {
         
        private readonly ILogger<LanguageController> _logger;

        public AuthorController(ILogger<LanguageController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{authorId}/tweet/take/{take}/skip/{skip}")] 
        public IActionResult GetTweetsByAuthorId(
            [FromServices] IReadRepository readRepository,
            string authorId,
            int take,
            int skip)
        { 
            return Ok(readRepository.GetByAuthorCount(authorId, take, skip));
        }
    }
}