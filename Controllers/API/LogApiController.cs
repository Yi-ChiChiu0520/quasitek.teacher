using Microsoft.AspNetCore.Mvc;
using quasitekWeb.Interface;
using quasitekWeb.Models;
using System.Threading.Tasks;

namespace quasitekWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogApiController : ControllerBase
    {
        private readonly ILogRepository _logRepository;

        public LogApiController(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        // POST: api/LogApi/upload-json
        [HttpPost("upload-json")]
        public async Task<IActionResult> UploadLogsFromJson([FromBody] LogWrapper logWrapper)
        {
            var result = await _logRepository.UploadLogsFromJson(logWrapper);

            if (result.Contains("error", System.StringComparison.InvariantCultureIgnoreCase))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
