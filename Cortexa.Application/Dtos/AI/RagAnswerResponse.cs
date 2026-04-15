namespace Cortexa.Application.Dtos.AI
{
    /// <summary>
    /// Response from the RAG /nlp/index/answer endpoint.
    /// The API returns a dynamic object; this captures the meaningful fields.
    /// </summary>
    public class RagAnswerResponse
    {
        public string? Answer { get; set; }
        public List<string> Sources { get; set; } = [];
    }
}
