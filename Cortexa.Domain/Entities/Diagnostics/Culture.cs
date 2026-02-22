using System;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Entities.Diagnostics
{
    public class Culture : BaseEntity
    {
        public CultureType CultureType { get; set; }
        public string Result { get; set; } = string.Empty; // "Positive", "Negative", or organism name
        public string? Sensitivity { get; set; } // Antibiotic sensitivity text or JSON
        public DateTime SampleDate { get; set; }

        public string AdmissionId { get; set; } = string.Empty;
        public Admission Admission { get; set; } = null!; // EF Core will set this

        public string DoctorId { get; set; } = string.Empty;
        public Doctor Doctor { get; set; } = null!; // EF Core will set this

        public string NurseId { get; set; } = string.Empty;
        public Nurse Nurse { get; set; } = null!; // EF Core will set this
    }
}
