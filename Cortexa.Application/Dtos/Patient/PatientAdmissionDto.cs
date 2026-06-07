using Cortexa.Application.Dtos.Clinical;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Dtos.Patient
{
    public class PatientAdmissionDto
    {
        public string AdmissionId { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string PatientId { get; set; } = string.Empty;
        public string FileNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public BloodType BloodType { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string InitialDiagnosis { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? DiagnosisSummary { get; set; }
        public string? BedId { get; set; }
        public string? RoomId { get; set; }

        public VitalSignsSummaryDto? LatestVitalSigns { get; set; }


    }
}
