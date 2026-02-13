using Cortexa.Domain.Common;
using Cortexa.Domain.Enums;


namespace Cortexa.Domain.Entities.SmartAssistant
{
    public class Alert : BaseEntity
    {
        public string AlertType { get; private set; }
        public AlertSeverity Severity { get; private set; }

        public DateTime GeneratedAt { get; private set; } = DateTime.UtcNow;
        public string Status { get; private set; } = "Active";

        public Guid AdmissionId { get; private set; }
        public Cortexa.Domain.Entities.Admission.Admission Admission { get; private set; }

        public ICollection<AlertOverrideLog> Overrides { get; private set; }
            = new List<AlertOverrideLog>();


    }
}
