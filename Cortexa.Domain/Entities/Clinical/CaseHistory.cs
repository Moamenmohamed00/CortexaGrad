using Cortexa.Domain.Common;

namespace Cortexa.Domain.Entities.Clinical
{
    public class CaseHistory : BaseEntity
    {
        public Guid AdmissionId { get; private set; }
        public Guid DoctorId { get; private set; }

        public string Complaint { get; private set; }
        public string PresentIllness { get; private set; }
        public string ChronicDisease { get; private set; }
        public string GeneticDisease { get; private set; }
        public string MaritalHistory { get; private set; }
        public string SpecialHabits { get; private set; }
        public string ClinicalNotes { get; private set; }

    }
}
