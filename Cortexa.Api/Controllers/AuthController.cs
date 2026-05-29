using Cortexa.Application.Dtos.Auth;
using Cortexa.Application.Features.Auth;
using Cortexa.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthController(ISender sender) : ApiControllerBase(sender)
    {

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="command">Login credentials.</param>
        /// <response code="200">Returns the authentication token and user info.</response>
        /// <response code="401">Invalid credentials provided.</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto command)
        {
            var result = await Sender.Send(new LoginCommand(command.Email, command.Password));
            return Ok(result);
        }

        /// <summary>
        /// Sends a password-reset OTP to the specified email.
        /// </summary>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto command)
        {
            var result = await Sender.Send(new ForgotPasswordCommand(command.Email));
            return Ok(result);
        }
        /// <summary>
        /// Resets the password using the provided OTP.
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto command)
        {
            var result = await Sender.Send(
                new ResetPasswordCommand(command.Email, command.Otp, command.NewPassword));

            return Ok(result);
        }
    }
}
