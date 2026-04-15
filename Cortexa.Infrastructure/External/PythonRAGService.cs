using Cortexa.Application.Dtos.AI;
using Cortexa.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Cortexa.Infrastructure.External
{
    public class PythonRAGService : IAIService
    {
        private readonly AIHttpClient _aiClient;
        private readonly ILogger<PythonRAGService> _logger;

        public PythonRAGService(AIHttpClient aiClient, ILogger<PythonRAGService> logger)
        {
            _aiClient = aiClient;
            _logger = logger;
        }

        public async Task<RagAnswerResponse> AskQuestionAsync(
            string projectId,
            string question,
            int limit = 5,
            CancellationToken ct = default)
        {
            var result = await _aiClient.AskAsync(projectId, question, limit, ct);

            if (result is null)
            {
                _logger.LogWarning("RAG returned no answer for project {Id}", projectId);
                return new RagAnswerResponse
                {
                    Answer = "AI assistant unavailable.",
                    Sources = []
                };
            }

            return result;
        }

        // ── Upload & Index Pipeline ────────────────────────────────────────
        public async Task<RagUploadResponse> UploadAndIndexDocumentAsync(
            string projectId,
            Stream fileStream,
            string fileName,
            CancellationToken ct = default)
        {
            var fileId = await _aiClient.UploadFileAsync(projectId, fileStream, fileName, ct);
            if (fileId is null)
                return new RagUploadResponse { Success = false, Message = "Upload failed." };

            var processed = await _aiClient.ProcessFileAsync(projectId, fileId, ct: ct);
            if (!processed)
                return new RagUploadResponse { Success = false, FileId = fileId, Message = "Processing failed." };

            var pushed = await _aiClient.PushToIndexAsync(projectId, ct: ct);
            if (!pushed)
                return new RagUploadResponse { Success = false, FileId = fileId, Message = "Index push failed." };

            _logger.LogInformation(
                "Document '{File}' fully indexed for project {Id}", fileName, projectId);

            return new RagUploadResponse
            {
                Success = true,
                FileId = fileId,
                Message = $"'{fileName}' uploaded and indexed successfully."
            };
        }

        // ── Index Info ─────────────────────────────────────────────────────
        public async Task<object?> GetIndexInfoAsync(string projectId, CancellationToken ct = default)
        {
            var raw = await _aiClient.GetIndexInfoAsync(projectId, ct);
            return raw is null ? null : (object)raw;
        }

        public Task<float> GenerateRiskScoreAsync(string patientId) => Task.FromResult(0f);
        public Task<AlertDto> GenerateAlertAsync(string admissionId, AlertDto alert) => Task.FromResult(alert);

        public async Task<RagQueryDto> ProcessRagQueryAsync(RagQueryDto query)
        {
            var admissionId = query.PatientId ?? "default_workspace"; // Use PatientId if admission not available in query
            var result = await AskQuestionAsync(admissionId, query.QueryText, 5);
            return query with
            {
                GeneratedResponse = result.Answer ?? "No response",
                ScoreTrust = 0.9f, 
                QueryDateTime = DateTime.UtcNow
            };
        }
    }
}
