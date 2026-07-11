using Cortexa.Application.Interfaces.Services;
using Cortexa.Infrastructure.Services;
using Cortexa.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Infrastructure.Services
{
    /// <summary>
    /// Stub notification service for non-SignalR scenarios (e.g., background jobs, testing).
    /// The primary SignalR-backed implementation lives in Cortexa.Api.Services.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public Task SendRealTimeAlertAsync(string userId, string alertType, string message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[Stub] Alert to {UserId}: [{AlertType}] {Message}", userId, alertType, message);
            return Task.CompletedTask;
        }

        public Task BroadcastVitalsUpdateAsync(string admissionId, object vitalsData, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[Stub] Vitals update for Admission {AdmissionId}", admissionId);
            return Task.CompletedTask;
        }

        public Task NotifyAdmissionStatusChangeAsync(string admissionId, string status, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[Stub] Admission status change for {AdmissionId}: {Status}", admissionId, status);
            return Task.CompletedTask;
        }

        public Task NotifyLabResultAvailableAsync(string admissionId, string testName, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[Stub] Lab result available for {AdmissionId}: {TestName}", admissionId, testName);
            return Task.CompletedTask;
        }

        public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[Stub] Sending Email to {To}: {Subject}", to, subject);
            return Task.CompletedTask;
        }

        public Task SendSmsAsync(string to, string message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[Stub] Sending SMS to {To}: {Message}", to, message);
            return Task.CompletedTask;
        }
    }
}
