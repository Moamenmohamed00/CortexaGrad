namespace Cortexa.Infrastructure.Services
{
    /// <summary>
    /// Strongly-typed email configuration settings.
    /// Bind from "EmailSettings" section in appsettings.json.
    /// </summary>
    public class EmailSettings
    {
        public const string SectionName = "EmailSettings";

        public string SmtpServer { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
