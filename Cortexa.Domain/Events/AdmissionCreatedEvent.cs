using System;

namespace Cortexa.Domain.Events
{
    /// <summary>
    /// Domain event raised when a new admission is created
    /// </summary>
    public class AdmissionCreatedEvent : DomainEvent
    {
        public string AdmissionId { get; }
        public string PatientId { get; }
        public string DoctorId { get; }
        public string? BedId { get; }
        public string? RoomId { get; }
        public DateTime AdmissionDate { get; }
        public string InitialDiagnosis { get; }

        public AdmissionCreatedEvent(
            string admissionId,
            string patientId,
            string doctorId,
            DateTime admissionDate,
            string initialDiagnosis,
            string? bedId = null,
            string? roomId = null)
        {
            AdmissionId = admissionId;
            PatientId = patientId;
            DoctorId = doctorId;
            AdmissionDate = admissionDate;
            InitialDiagnosis = initialDiagnosis;
            BedId = bedId;
            RoomId = roomId;
        }
    }
}
