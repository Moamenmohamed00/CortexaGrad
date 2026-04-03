using System;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Enums;

namespace Cortexa.Domain.Entities.Clinical
{
    public class VitalSigns : BaseEntity, IAuditableEntity
    {
        public DateTime RecordedAt { get; set; }
        public float Temperature { get; set; }
        public int HeartRate { get; set; }
        public int RespRate { get; set; }

        // Use standard PascalCase to match DTOs and C# conventions
        public int BpSystolic { get; set; }
        public int BpDiastolic { get; set; }
        public int PulseOxy { get; set; }
        public int Cvp { get; set; }
        public string InsulinGiven { get; set; } = string.Empty;

        public int GcsEye { get; set; }
        public int GcsVerbal { get; set; }
        public int GcsMotor { get; set; }
        public int GcsTotal => GcsEye + GcsVerbal + GcsMotor;

        public bool SupplementalOxygen { get; set; }
        public ConsciousnessLevel ConsciousnessLevel { get; set; }
        public int NewsScore { get; set; }
        public NewsRiskLevel NewsRiskLevel { get; set; }

        public string AdmissionId { get; set; } = string.Empty;
        public Admission Admission { get; set; } = null!;

        public string NurseId { get; set; } = string.Empty;
        public Nurse Nurse { get; set; } = null!;

        public string? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
    }
}

