using System.ComponentModel.DataAnnotations;

namespace Cortexa.Application.Dtos.Auth
{
    /// <summary>
    /// Request DTO for resetting a password using an OTP.
    /// </summary>
    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Otp { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
