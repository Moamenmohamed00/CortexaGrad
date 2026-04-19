namespace Cortexa.Application.Dtos.AI
{
    /// <summary>
    /// Response returned to the API consumer after uploading and indexing a document.
    /// </summary>
    public class RagUploadResponse
    {
        public bool Success { get; set; }
        public string? FileId { get; set; }
        public string? Message { get; set; }
    }
}
