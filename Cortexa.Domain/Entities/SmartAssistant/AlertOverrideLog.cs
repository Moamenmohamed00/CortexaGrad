using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Identity;

namespace Cortexa.Domain.Entities.SmartAssistant
{
    public class AlertOverrideLog : BaseEntity
    {

        public Guid? ProcedureId { get; private set; }

        public string Reason { get; private set; }
        public DateTime OverrideTime { get; private set; } = DateTime.UtcNow;

        public Guid AlertId { get; private set; }
        public Alert Alert { get; private set; }

        public Guid DoctorId { get; private set; }
        public Doctor Doctor { get; private set; }

    }
}
