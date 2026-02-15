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
            var userId = await Sender.Send(new RegisterCommand(request));
            return Ok(new { userId });
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
            await Sender.Send(new ForgotPasswordCommand(request.Email));
            return Ok(new { message = "If the email exists, a reset OTP has been sent." });
        }

        /// <summary>
        /// Resets the password using the provided OTP.
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            var success = await Sender.Send(new ResetPasswordCommand(request.Email, request.Otp, request.NewPassword));
            return success
                ? Ok(new { message = "Password has been reset successfully." })
                : BadRequest(new { message = "Invalid or expired OTP." });
        }
    }
}
