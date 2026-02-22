using System;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Dtos.Diagnostics
{
    public record CultureDto(
        string Id,
        CultureType CultureType,
        string Result,
        string? Sensitivity,
        DateTime SampleDate,
        string AdmissionId,
        string DoctorId,
        string NurseId
    );
}
