using Microsoft.AspNetCore.SignalR;

namespace Cortexa.Api.Hubs
{
    /// <summary>
    /// SignalR Hub for real-time patient monitoring (vitals streaming, fluid balance updates).
    /// Clients join admission-specific groups to receive live data feeds.
    /// </summary>
    public class MonitoringHub : Hub
    {
        public async Task JoinMonitoring(string admissionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"monitoring-{admissionId}");
        }

        public async Task LeaveMonitoring(string admissionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"monitoring-{admissionId}");
        }
    }
}
