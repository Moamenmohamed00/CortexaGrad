using Cortexa.Application.Dtos.AI;

namespace Cortexa.Application.Interfaces.Services
{
    public interface IAIService
    {
        // ── RAG Ask ────────────────────────────────────────────────────────
        /// <summary>
        /// Automatically fetches the patient's clinical data for the given admission,
        /// builds context, and sends the question + context to the RAG model.
        /// The projectId determines which document workspace to query.
        /// </summary>
        Task<RagAnswerResponse> AskQuestionAsync(
            string projectId,
            string admissionId,
            string question,
            int limit = 5,
            CancellationToken ct = default);

        // ── RAG Upload ─────────────────────────────────────────────────────
        /// <summary>
        /// Full pipeline: uploads the file, processes it into chunks, then pushes to index.
        /// </summary>
        Task<RagUploadResponse> UploadAndIndexDocumentAsync(
            string projectId,
            Stream fileStream,
            string fileName,
            CancellationToken ct = default);

        // ── RAG Index Info ─────────────────────────────────────────────────
        /// <summary>
        /// Returns index metadata for the given project workspace.
        /// </summary>
        Task<object?> GetIndexInfoAsync(
            string projectId,
            CancellationToken ct = default);

        // ── Legacy / kept for backward compat ──────────────────────────────
        Task<float> GenerateRiskScoreAsync(string patientId);
        Task<AlertDto> GenerateAlertAsync(string admissionId, AlertDto alert);
        Task<RagQueryDto> ProcessRagQueryAsync(RagQueryDto query);
    }
}
