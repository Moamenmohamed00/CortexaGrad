using System.ComponentModel.DataAnnotations;

namespace Cortexa.Application.Dtos.Auth
{
    /// <summary>
    /// Request DTO for forgot-password flow (sends OTP to email).
    /// </summary>
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
