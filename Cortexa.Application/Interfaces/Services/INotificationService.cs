using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Interfaces.Services
{
    public interface INotificationService
    {
        // Real-time SignalR notifications
        Task SendRealTimeAlertAsync(string userId, string alertType, string message, CancellationToken cancellationToken = default);
        Task BroadcastVitalsUpdateAsync(string admissionId, object vitalsData, CancellationToken cancellationToken = default);
        Task NotifyAdmissionStatusChangeAsync(string admissionId, string status, CancellationToken cancellationToken = default);
        Task NotifyLabResultAvailableAsync(string admissionId, string testName, CancellationToken cancellationToken = default);

        // Traditional notifications
        Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
        //Task SendSmsAsync(string to, string message, CancellationToken cancellationToken = default);
        /*ابعت كورس السيجنال ار يا عبد الغني*/
    }
}
