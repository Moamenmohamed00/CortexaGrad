using System;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Dtos.Diagnostics
{
    public record ImagingDto(
        string Id,
        ImagingType Type,
        string? Findings,
        DateTime Date,
        string AdmissionId,
        string DoctorId
    );
}
