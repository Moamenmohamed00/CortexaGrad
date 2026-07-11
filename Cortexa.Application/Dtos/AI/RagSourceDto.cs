using System.Text.Json.Serialization;

namespace Cortexa.Application.Dtos.AI
{
    /// <summary>
    /// Represents a single retrieved source chunk from the RAG model,
    /// including the file name and page number so the frontend
    /// can build a direct link (e.g. PDF viewer at a specific page).
    /// </summary>
    public class RagSourceDto
    {
        [JsonPropertyName("doc_no")]
        public int DocNo { get; set; }

        [JsonPropertyName("citation_tag")]
        public string CitationTag { get; set; } = string.Empty;

        [JsonPropertyName("source_id")]
        public string SourceId { get; set; } = string.Empty;

        [JsonPropertyName("source_file_name")]
        public string SourceFileName { get; set; } = string.Empty;

        [JsonPropertyName("page_number")]
        public int? PageNumber { get; set; }

        [JsonPropertyName("chunk_index_in_page")]
        public int? ChunkIndexInPage { get; set; }

        [JsonPropertyName("chunk_char_count")]
        public int? ChunkCharCount { get; set; }

        [JsonPropertyName("similarity_score")]
        public float SimilarityScore { get; set; }

        /// <summary>
        /// Direct link to the exact page of the source PDF on HuggingFace.
        /// Computed server-side — not returned by the FastAPI.
        /// Example: https://huggingface.co/spaces/{space}/resolve/main/src/assets/files/{projectId}/file.pdf#page=129
        /// </summary>
        public string? Link { get; set; }
    }
}
