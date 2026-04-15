namespace Cortexa.Application.Dtos.AI
{
    /// <summary>
    /// Maps to the RAG API's "searchRequest" schema.
    /// Used for both /nlp/index/search and /nlp/index/answer endpoints.
    /// </summary>
    public class RagSearchRequest
    {
        public string Text { get; set; } = string.Empty;
        public int Limit { get; set; } = 5;
    }
}
