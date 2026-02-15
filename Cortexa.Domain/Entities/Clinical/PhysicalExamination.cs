using System;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;

namespace Cortexa.Domain.Entities.Clinical
{
    public class PhysicalExamination : BaseEntity
    {
        public DateTime ExamDate { get; set; }
        public float Temperature { get; set; }
        public string BloodPressure { get; set; } = string.Empty;
        public int Pulse { get; set; }
        public int RespRate { get; set; }

        public string EyeStatus { get; set; } = string.Empty;
        public string SkinStatus { get; set; } = string.Empty;
        public string LipsStatus { get; set; } = string.Empty;

        public string HeartExam { get; set; } = string.Empty;
        public string AbdomenExam { get; set; } = string.Empty;
        public string LocalExamination { get; set; } = string.Empty;

        public string AdmissionId { get; set; } = string.Empty;
        public Admission Admission { get; set; } = null!; // EF Core will set this

        public string DoctorId { get; set; } = string.Empty;
        public Doctor Doctor { get; set; } = null!; // EF Core will set this
    }
}
