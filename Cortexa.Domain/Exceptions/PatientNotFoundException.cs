using System;

namespace Cortexa.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when a patient cannot be found in the system
    /// </summary>
    public class PatientNotFoundException : DomainException
    {
        public string? PatientId { get; }
        public string? FileNumber { get; }

        public PatientNotFoundException()
            : base("Patient not found.", 404, "Patient Not Found")
        {
        }

        public PatientNotFoundException(string patientId)
            : base($"Patient with ID '{patientId}' was not found.", 404, "Patient Not Found")
        {
            PatientId = patientId;
        }

        public PatientNotFoundException(string patientId, string fileNumber)
            : base($"Patient with ID '{patientId}' and File Number '{fileNumber}' was not found.", 404, "Patient Not Found")
        {
            PatientId = patientId;
            FileNumber = fileNumber;
        }

        public PatientNotFoundException(string patientId, Exception innerException)
            : base($"Patient with ID '{patientId}' was not found.", 404, "Patient Not Found")
        {
            PatientId = patientId;
        }
    }
}
