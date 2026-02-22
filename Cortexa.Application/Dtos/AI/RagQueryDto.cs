using System;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Dtos.AI
{
    public record RagQueryDto(
        string Id,
        string QueryText,
        float ScoreTrust,
        RelevanceLevel RelevanceLevel,
        DateTime QueryDateTime,
        string GeneratedResponse,
        string DoctorId,
        string? PatientId
    );
}
