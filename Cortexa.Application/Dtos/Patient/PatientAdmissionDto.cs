namespace Cortexa.Application.Dtos.Patient
{
    public class PatientAdmissionDto
    {
        public string PatientId { get; set; } = string.Empty;
        public string FileNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string AdmissionId { get; set; } = string.Empty;
        public DateTime AdmissionDate { get; set; }
        public string InitialDiagnosis { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? BedId { get; set; }
        public string? RoomId { get; set; }
    }
}
