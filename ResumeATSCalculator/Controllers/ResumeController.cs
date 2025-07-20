using Microsoft.AspNetCore.Mvc;
using ResumeATSCalculator.Models;
using ResumeATSCalculator.Services;

namespace ResumeATSCalculator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly ResumeService _service;

        public ResumeController(ResumeService service)
        {
            _service = service;
        }

        [HttpPost("score")]
        public async Task<IActionResult> Score([FromBody] Resume resume)
        {
            var result = await _service.ScoreResumeAsync(resume.Content);
            return Ok(result);
        }
    }

}
