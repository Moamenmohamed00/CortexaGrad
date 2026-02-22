using System.Net;
using System.Net.Mail;
using Cortexa.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cortexa.Infrastructure.Services
{
    /// <summary>
    /// Email service using SMTP (Gmail) with settings from appsettings.json.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Sending email to {To} | Subject: {Subject}",
                to, subject);

            try
            {
                using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
                {
                    Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage, cancellationToken);

                _logger.LogInformation("Email sent successfully to {To}", to);
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", to);
                throw;
            }
        }
    }
}
