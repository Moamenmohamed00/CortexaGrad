using System;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Dtos.AI
{
    public record AlertDto(
        string Id,
        string PatientName,
        string AlertMessage,
        AlertSeverity Severity,
        DateTime GeneratedAt,
        AlertStatus Status,
        string AdmissionId
    );
}
