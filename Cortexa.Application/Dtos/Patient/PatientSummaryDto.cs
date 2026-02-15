using System;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Dtos.Patient
{
    public class PatientSummaryDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FileNumber { get; set; } = string.Empty;
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public AdmissionStatus? CurrentAdmissionStatus { get; set; }
    }
}
