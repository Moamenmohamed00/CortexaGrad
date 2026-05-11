using System;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Dtos.AI
{
    public record RagQueryDto
    {
        public string Id { get; init; } = string.Empty;
        public string QueryText { get; init; } = string.Empty;
        public float ScoreTrust { get; init; }
        public RelevanceLevel RelevanceLevel { get; init; }
        public DateTime QueryDateTime { get; init; }
        public string GeneratedResponse { get; init; } = string.Empty;
        public string DoctorId { get; init; } = string.Empty;
        public string? PatientId { get; init; }
        public List<string> Sources { get; init; } = [];
    }
}
