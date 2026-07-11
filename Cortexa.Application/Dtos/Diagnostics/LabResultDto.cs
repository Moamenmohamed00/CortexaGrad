using System;

namespace Cortexa.Application.Dtos.Diagnostics
{
    public record LabResultDto(
        string Id,
        string Parameter,
        float Value,
        string Unit,
        string? ReferenceRange,
        DateTime SampleDate,
        string LabOrderId,
        string NurseId
    );
}
