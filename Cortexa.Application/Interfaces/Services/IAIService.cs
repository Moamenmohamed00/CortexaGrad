using Cortexa.Application.Dtos.AI;

namespace Cortexa.Application.Interfaces.Services
{
    public interface IAIService
    {
        // ── RAG Ask ────────────────────────────────────────────────────────
        /// <summary>
        /// Sends a natural language question to the RAG model scoped to an admission's
        /// document workspace and returns a grounded AI answer.
        /// </summary>
        Task<RagAnswerResponse> AskQuestionAsync(
            string admissionId,
            string question,
            int limit = 5,
            CancellationToken ct = default);

        // ── RAG Upload ─────────────────────────────────────────────────────
        /// <summary>
        /// Full pipeline: uploads the file, processes it into chunks, then pushes to index.
        /// Uses admissionId as the project_id (workspace).
        /// </summary>
        Task<RagUploadResponse> UploadAndIndexDocumentAsync(
            string admissionId,
            Stream fileStream,
            string fileName,
            CancellationToken ct = default);

        // ── RAG Index Info ─────────────────────────────────────────────────
        /// <summary>
        /// Returns index metadata for the given admission workspace.
        /// </summary>
        Task<object?> GetIndexInfoAsync(
            string admissionId,
            CancellationToken ct = default);

        // ── Legacy / kept for backward compat ──────────────────────────────
        Task<float> GenerateRiskScoreAsync(string patientId);
        Task<AlertDto> GenerateAlertAsync(string admissionId, AlertDto alert);
        Task<RagQueryDto> ProcessRagQueryAsync(RagQueryDto query);
    }
}
