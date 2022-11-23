using JwtAuth.API.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SerilogTimings;

namespace JwtAuth.API.Controllers
{
    [Authorize(Roles = nameof(AuthorizationRoles.Dolphin))]
    [ApiController]
    [Route("api/[controller]")]
    public class EchoController : ControllerBase
    {
        private readonly ILogger<EchoController> _logger;
        public EchoController(ILogger<EchoController> logger)
        {
            _logger = logger;
        }

        [HttpGet("get")]
        public ActionResult<string> EchoGet(string message)
        {
            using (Operation.Time("Submitting message: {message}"))
            {
                return Ok($"You're message is {message}");
            }
        }

        [HttpPost("post")]
        public ActionResult<string> EchoPost([FromBody] string message)
        {
            return Ok($"You're message is {message}");
        }
    }
}