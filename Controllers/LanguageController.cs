using Microsoft.AspNetCore.Mvc;
using Twitter.Data; 

namespace Twitter.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LanguageController : ControllerBase
    {
         
        private readonly ILogger<LanguageController> _logger;

        public LanguageController(ILogger<LanguageController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{languageCode}/tweet/take/{take}/skip/{skip}")] 
        public IActionResult GetTweetsByLanguagesCode(
            [FromServices] IReadRepository readRepository,
            string languageCode,
            int take,
            int skip)
        { 
            return Ok(readRepository.GetLanguageCount(languageCode, take,skip));
        }

        [HttpGet("orderbycount")]
        public IActionResult GetTweetsLanguagesOrderByCount([FromServices] IReadRepository readRepository)
        {
            return Ok(readRepository.GetLanguageCount().OrderByDescending(e=>e.Count));
        }

        [HttpGet("orderbyalphabetical")]
        public IActionResult GetTweetsLanguagesByAlphabet([FromServices] IReadRepository readRepository)
        {
            return Ok(readRepository.GetLanguageCount().OrderBy(e => e.Language));
        }
    }
}