namespace Cortexa.Application.Dtos.Auth
{
    /// <summary>
    /// Response DTO returned after successful authentication.
    /// </summary>
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;

        public string UserIdInSystem { get; set; } = string.Empty; // ID of the corresponding domain user (e.g., PatientId, DoctorId)
        public List<string> Roles { get; set; } = new();
    }
}
