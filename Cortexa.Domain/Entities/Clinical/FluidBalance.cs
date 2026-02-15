using System;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Entities.Clinical
{
    public class FluidBalance : BaseEntity
    {
        public DateTime RecordedAt { get; set; }
        public FluidBalanceCategory Category { get; set; } // Intake/OutputEnum

        public FluidType Type { get; set; } // "Oral", "IV", "Urine", "Drain"
        public int Amount_ML { get; set; }

        public string AdmissionId { get; set; } = string.Empty;
        public Admission Admission { get; set; } = null!; // EF Core will set this

        public string NurseId { get; set; } = string.Empty;
        public Nurse Nurse { get; set; } = null!; // EF Core will set this
    }
}
