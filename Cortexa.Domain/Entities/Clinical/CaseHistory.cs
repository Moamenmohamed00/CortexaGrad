using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;

namespace Cortexa.Domain.Entities.Clinical
{
    public class CaseHistory : BaseEntity
    {
        public string Complaint { get; set; } = string.Empty;
        public string PresentIllness { get; set; } = string.Empty;
        public string? ChronicDisease { get; set; } // Could be List<string> for future
        public string? GeneticDisease { get; set; }
        public string? MaritalHistory { get; set; }
        public string? SpecialHabits { get; set; }
        public string? ClinicalNotes { get; set; }

        public string AdmissionId { get; set; } = string.Empty;
        public Admission Admission { get; set; } = null!; // EF Core will set this

        public string DoctorId { get; set; } = string.Empty;
        public Doctor Doctor { get; set; } = null!; // EF Core will set this
    }
}
