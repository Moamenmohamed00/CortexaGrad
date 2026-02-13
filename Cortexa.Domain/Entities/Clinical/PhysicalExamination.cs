using Cortexa.Domain.Common;

namespace Cortexa.Domain.Entities.Clinical
{
    public class PhysicalExamination : BaseEntity
    {
        public Guid AdmissionId { get; private set; }
        public Guid DoctorId { get; private set; }

        public float Temperature { get; private set; }
        public string BloodPressure { get; private set; }
        public int Pulse { get; private set; }
        public int RespRate { get; private set; }

        public string EyeStatus { get; private set; }
        public string SkinStatus { get; private set; }
        public string LipsStatus { get; private set; }
        public string HeartExam { get; private set; }
        public string AbdomenExam { get; private set; }
        public string LocalExamination { get; private set; }

        public DateTime ExamDate { get; private set; }

    }
}
