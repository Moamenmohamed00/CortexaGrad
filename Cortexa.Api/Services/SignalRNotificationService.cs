using Cortexa.Api.Hubs;
using Cortexa.Application.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Api.Services
{
    /// <summary>
    /// SignalR-backed notification service. Lives in the Api layer 
    /// because it requires IHubContext references which are only available here.
    /// Implements the Application-layer INotificationService interface.
    /// </summary>
    public class SignalRNotificationService : INotificationService
    {
        private readonly IHubContext<AlertHub> _alertHubContext;
        private readonly IHubContext<MonitoringHub> _monitoringHubContext;
        private readonly ILogger<SignalRNotificationService> _logger;

        public SignalRNotificationService(
            IHubContext<AlertHub> alertHubContext,
            IHubContext<MonitoringHub> monitoringHubContext,
            ILogger<SignalRNotificationService> logger)
        {
            _alertHubContext = alertHubContext;
            _monitoringHubContext = monitoringHubContext;
            _logger = logger;
        }

        public async Task SendRealTimeAlertAsync(string userId, string alertType, string message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending real-time alert to User {UserId}: [{AlertType}] {Message}", userId, alertType, message);
            await _alertHubContext.Clients.Group($"user-{userId}")
                .SendAsync("ReceiveAlert", alertType, message, cancellationToken);
        }

        public async Task BroadcastVitalsUpdateAsync(string admissionId, object vitalsData, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Broadcasting vitals update for Admission {AdmissionId}", admissionId);
            await _monitoringHubContext.Clients.Group($"monitoring-{admissionId}")
                .SendAsync("ReceiveVitalsUpdate", vitalsData, cancellationToken);
        }

        public async Task NotifyAdmissionStatusChangeAsync(string admissionId, string status, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Notifying admission status change for {AdmissionId}: {Status}", admissionId, status);
            await _alertHubContext.Clients.Group($"admission-{admissionId}")
                .SendAsync("AdmissionStatusChanged", admissionId, status, cancellationToken);
        }

        public async Task NotifyLabResultAvailableAsync(string admissionId, string testName, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Notifying lab result available for {AdmissionId}: {TestName}", admissionId, testName);
            await _alertHubContext.Clients.Group($"admission-{admissionId}")
                .SendAsync("LabResultAvailable", admissionId, testName, cancellationToken);
        }

        public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending Email to {To}: {Subject}", to, subject);
            // TODO: Integrate with actual email provider (SendGrid, SMTP, etc.)
            return Task.CompletedTask;
        }

        public Task SendSmsAsync(string to, string message, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending SMS to {To}: {Message}", to, message);
            // TODO: Integrate with actual SMS provider (Twilio, etc.)
            return Task.CompletedTask;
        }
    }
}
