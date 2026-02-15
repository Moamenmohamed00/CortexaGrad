using System;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Events
{
    /// <summary>
    /// Domain event raised when an alert is triggered for a patient admission
    /// </summary>
    public class AlertTriggeredEvent : DomainEvent
    {
        public string AlertId { get; }
        public string AdmissionId { get; }
        public string AlertMessage { get; }
        public AlertSeverity Severity { get; }
        public DateTime GeneratedAt { get; }

        public AlertTriggeredEvent(
            string alertId,
            string admissionId,
            string alertMessage,
            AlertSeverity severity,
            DateTime generatedAt)
        {
            AlertId = alertId;
            AdmissionId = admissionId;
            AlertMessage = alertMessage;
            Severity = severity;
            GeneratedAt = generatedAt;
        }
    }
}
