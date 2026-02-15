namespace Cortexa.Infrastructure.Identity
{
    /// <summary>
    /// Strongly-typed JWT configuration settings.
    /// Bind from "JwtSettings" section in appsettings.json.
    /// </summary>
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";

        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = "Cortexa";
        public string Audience { get; set; } = "CortexaUsers";
        public int DurationInMinutes { get; set; } = 60;
    }
}
