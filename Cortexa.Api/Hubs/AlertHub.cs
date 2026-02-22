using Microsoft.AspNetCore.SignalR;

namespace Cortexa.Api.Hubs
{
    /// <summary>
    /// SignalR Hub for clinical alerts (AI-generated alerts, critical vitals, lab results).
    /// Clients join admission-specific groups to receive targeted notifications.
    /// </summary>
    public class AlertHub : Hub
    {
        public async Task JoinAdmissionGroup(string admissionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"admission-{admissionId}");
        }

        public async Task LeaveAdmissionGroup(string admissionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"admission-{admissionId}");
        }

        public async Task JoinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
        }

        public async Task LeaveUserGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-{userId}");
        }
    }
}
