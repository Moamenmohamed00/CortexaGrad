namespace Cortexa.Application.Common.Models
{
    /// <summary>
    /// Represents the result of an authentication attempt.
    /// </summary>
    public record AuthResult
    {
        public bool Succeeded { get; init; }
        public string? Token { get; init; }
        public string? Error { get; init; }

        public static AuthResult Success(string token) =>
            new() { Succeeded = true, Token = token };

        public static AuthResult Failure(string error) =>
            new() { Succeeded = false, Error = error };
    }

    /// <summary>
    /// Lightweight DTO for user identity information.
    /// </summary>
    public record UserInfo(
        string Id,
        string Name,
        string Email,
        IEnumerable<string> Roles
    );
}
