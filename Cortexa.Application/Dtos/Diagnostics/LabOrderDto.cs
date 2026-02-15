using System;
using System.Collections.Generic;

namespace Cortexa.Application.Dtos.Diagnostics
{
    public record LabOrderDto(
        string Id,
        string TestName,
        DateTime OrderDate,
        string AdmissionId,
        string DoctorId,
        List<LabResultDto> Results
    );
}
