using System;
using System.Collections.Generic;

namespace Cortexa.Application.DTOs.AI
{
    public class RAGResponseDto
    {
        public string QueryId { get; set; } = string.Empty;
        public string QueryText { get; set; } = string.Empty;
        public string GeneratedResponse { get; set; } = string.Empty;
        public float ConfidenceScore { get; set; }
        public DateTime GeneratedAt { get; set; }
        public List<string> SourceReferences { get; set; } = new();
    }
}
