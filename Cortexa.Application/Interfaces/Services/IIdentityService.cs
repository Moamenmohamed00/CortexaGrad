using Cortexa.Application.Dtos.Auth;
using Cortexa.Application.Dtos.Core;

namespace Cortexa.Application.Interfaces.Services
{
    /// <summary>
    /// Service interface for identity operations: authentication, registration,
    /// and password management. Implemented in the Infrastructure layer.
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Authenticates a user with email and password, returning a JWT token and user info.
        /// </summary>
        Task<ResultDto<AuthResponseDto>> LoginAsync(string email, string password);

        /// <summary>
        /// Registers a new user with role-specific data and returns the created user ID.
        /// </summary>
        Task<ResultDto<string>> RegisterAsync(RegisterRequestDto request);

        /// <summary>
        /// Sends a password-reset OTP to the specified email address.
        /// </summary>
        Task<ResultDto<bool>> SendPasswordResetOtpAsync(string email);

        /// <summary>
        /// Resets a user's password using the provided OTP.
        /// </summary>
        Task<ResultDto<bool>> ResetPasswordWithOtpAsync(string email, string otp, string newPassword);
    }
}
