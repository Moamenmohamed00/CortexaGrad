using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cortexa.Application.Dtos.AI
{
    /// <summary>
    /// Full response from the RAG /nlp/index/answer endpoint.
    /// Now includes rich source metadata so the client can link
    /// to the exact page of the referenced document.
    /// </summary>
    public class RagAnswerResponse
    {
        // ── Core answer fields ───────────────────────────────────────
        [JsonPropertyName("signal")]
        public string? Signal { get; set; }

        [JsonPropertyName("answer")]
        public string? Answer { get; set; }

        [JsonPropertyName("clinical_status")]
        public string? ClinicalStatus { get; set; }

        [JsonPropertyName("evidence_synthesis")]
        public string? EvidenceSynthesis { get; set; }

        [JsonPropertyName("missing_investigations")]
        public string? MissingInvestigations { get; set; }

        // ── Simple citation tags e.g. ["doc_1","doc_2"] ─────────────
        [JsonPropertyName("sources")]
        public List<string> Sources { get; set; } = [];

        // ── Full source objects with page numbers ────────────────────
        [JsonPropertyName("retrieved_sources")]
        public List<RagSourceDto> RetrievedSources { get; set; } = [];
    }
}
