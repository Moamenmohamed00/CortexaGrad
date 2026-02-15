using System;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;

namespace Cortexa.Domain.Entities.Clinical
{
    public class VitalSigns : BaseEntity
    {
        public DateTime RecordedAt { get; set; }
        public float Temperature { get; set; }
        public int HeartRate { get; set; }
        public int RespRate { get; set; }
        public int BP_Systolic { get; set; }
        public int BP_Diastolic { get; set; }
        public int PulseOxy { get; set; } // SpO2
        public int CVP { get; set; } // Central Venous Pressure
        public string InsulinGiven { get; set; } = string.Empty; // Could be decimal or string

        // Glasgow Coma Scale
        public int GCS_Eye { get; set; }
        public int GCS_Verbal { get; set; }
        public int GCS_Motor { get; set; }
        public int GCS_Total => GCS_Eye + GCS_Verbal + GCS_Motor;

        // Relationships
        public string AdmissionId { get; set; } = string.Empty;
        public Admission Admission { get; set; } = null!; // EF Core will set this

        public string NurseId { get; set; } = string.Empty;
        public Nurse Nurse { get; set; } = null!; // EF Core will set this

        // Optional Verification by Doctor
        public string? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
