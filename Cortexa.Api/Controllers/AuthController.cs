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
        /// Adds a new user to the system using the specified user details.
        /// </summary>
        /// <remarks>This action requires the caller to have the Admin role.</remarks>
        /// <param name="command">An object containing the information required to create the new user. Cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the operation. Returns status code 200 (OK) if
        /// the user is added successfully; otherwise, returns status code 400 (Bad Request) if the input is invalid.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = $"{AppRoles.Admin}")]
        [HttpPost("add-user")]

        public async Task<IActionResult> AddUser([FromBody] AddUserRequestDto command)
        {
            var result = await Sender.Send(new AddUserCommand(command));
            return Ok(result);
        }

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
