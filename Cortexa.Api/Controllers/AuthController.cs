using Cortexa.Application.Dtos.Auth;
using Cortexa.Application.Features.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    [AllowAnonymous]
    public class AuthController : ApiControllerBase
    {
        /// <summary>
        /// Registers a new user account.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await Sender.Send(new RegisterCommand(request));
            return Ok(result);
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await Sender.Send(new LoginCommand(request.Email, request.Password));
            return Ok(result);
        }

        /// <summary>
        /// Sends a password-reset OTP to the specified email.
        /// </summary>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            var result = await Sender.Send(new ForgotPasswordCommand(request.Email));
            return Ok(result);
        }
        /// <summary>
        /// Resets the password using the provided OTP.
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            var result = await Sender.Send(
                new ResetPasswordCommand(request.Email, request.Otp, request.NewPassword));

            return Ok(result);
        }
    }
}
