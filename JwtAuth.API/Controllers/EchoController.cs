using JwtAuth.API.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuth.API.Controllers
{
    [Authorize(Roles = nameof(AuthorizationRoles.Dolphin))]
    [Route("api/[controller]")]
    public class EchoController : ControllerBase
    {
        [HttpGet("get")]
        public ActionResult<string> EchoGet(string message)
        {
            return Ok($"You're message is {message}");
        }

        [HttpPost("post")]
        public ActionResult<string> EchoPost([FromBody] string message)
        {
            return Ok($"You're message is {message}");
        }
    }
}