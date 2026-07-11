using System;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Dtos.Core
{
    public record AdmissionDto(
        string Id,
        DateTime AdmissionDate,
        DateTime? DischargeDate,
        string InitialDiagnosis,
        AdmissionStatus Status,
        string PatientId,
        string DoctorId,
        string? RoomId,
        string? BedId
    );
}
