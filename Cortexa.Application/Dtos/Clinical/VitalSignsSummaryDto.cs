namespace Cortexa.Application.Dtos.Clinical
{
    public class VitalSignsSummaryDto
    {
        public float Temperature { get; set; }
        public int HeartRate { get; set; }
        public int RespRate { get; set; }

        public int BpSystolic { get; set; }
        public int BpDiastolic { get; set; }

        public int PulseOxy { get; set; }

        public int GcsTotal { get; set; }

        public DateTime RecordedAt { get; set; }
    }
}

