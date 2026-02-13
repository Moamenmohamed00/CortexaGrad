using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Admission;
using Cortexa.Domain.Entities.Identity;

namespace Cortexa.Domain.Entities.SmartAssistant
{
    public class RAGQuery : BaseEntity
    {
        public Guid DoctorId { get; private set; }
        public Doctor Doctor { get; private set; }

        public Guid PatientId { get; private set; }
        public Patient Patient { get; private set; }


        public string QueryText { get; private set; }
        public float ScoreTrust { get; private set; }
        public string RelevanceLevel { get; private set; }
        public DateTime QueryDateTime { get; private set; }

    }
}
