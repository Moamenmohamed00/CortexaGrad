using System;
using System.Collections.Generic;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Entities.AI
{
    public class Alert : BaseEntity
    {
        /*send message if vital not regular*/
        public string AlertMessage { get; set; } = string.Empty; // e.g. "Sepsis Warning", "Drug Interaction"
        public AlertSeverity Severity { get; set; }
        public DateTime GeneratedAt { get; set; }
        public AlertStatus Status { get; set; }

        public string AdmissionId { get; set; }
        public Admission Admission { get; set; } = null!; // EF Core will set this

        // Navigation Properties
        public ICollection<AlertOverrideLog> AlertOverrideLogs { get; set; } = new List<AlertOverrideLog>();
    }
}
