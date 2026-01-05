using Microsoft.AspNetCore.Mvc;
using TCM.Application.Features.Authentication.Commands.Token;

namespace TCM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region Private Variables
        private readonly IMediator _mediator;
        #endregion

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Login
        /// <summary>
        /// Login
        /// </summary>
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
                return BadRequest("User not authenticated");

            long userId = !string.IsNullOrEmpty(userIdClaim) ? Convert.ToInt64(userIdClaim) : 0;

            // Call CQRS command
            var command = new GenerateTokenCommand(userId);

            var result = await _mediator.Send(command);

            return Ok(result);
        }
        #endregion

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<ActionResult> RefreshToken([FromHeader(Name = "X-Refresh-Token")] string refreshToken)
        {
            var request = HttpContext.Request;
            var command = new GenerateTokenByRefreshTokenCommand(refreshToken);
            var response = await _mediator.Send(command);

            return Ok(response);
        }
    }
}
