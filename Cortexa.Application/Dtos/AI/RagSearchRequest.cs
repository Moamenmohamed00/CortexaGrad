using System.Text.Json.Serialization;

namespace Cortexa.Application.Dtos.AI
{
    /// <summary>
    /// Maps to the RAG API's "searchRequest" schema.
    /// Used for both /nlp/index/search and /nlp/index/answer endpoints.
    /// </summary>
    public class RagSearchRequest
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("limit")]
        public int Limit { get; set; } = 5;
    }
}
