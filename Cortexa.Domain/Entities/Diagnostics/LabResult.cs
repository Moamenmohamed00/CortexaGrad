using System;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;

namespace Cortexa.Domain.Entities.Diagnostics
{
    public class LabResult : BaseEntity
    {
        public string Parameter { get; set; } = string.Empty; // e.g. "Hb", "WBC"
        public float Value { get; set; }
        public string Unit { get; set; } = string.Empty; // mg/dL, g/L, etc.
        public string? ReferenceRange { get; set; }
        public DateTime SampleDate { get; set; }

        public string LabOrderId { get; set; } = string.Empty;
        public LabOrder LabOrder { get; set; } = null!; // EF Core will set this

        public string NurseId { get; set; } = string.Empty; // Entered by / Sample taken by
        public Nurse Nurse { get; set; } = null!; // EF Core will set this
    }
}
