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

        // ── Citation tags e.g. ["doc_1","doc_2"] ────────────────────
        public List<string> Sources { get; init; } = [];

        // ── Full source objects with file name + page number ─────────
        // Allows the frontend to build a direct link to the exact book page.
        public List<RagSourceDto> RetrievedSources { get; init; } = [];

        // ── Additional clinical fields from the FastAPI ───────────────
        public string? ClinicalStatus { get; init; }
        public string? EvidenceSynthesis { get; init; }
        public string? MissingInvestigations { get; init; }
    }
}
