using System.Net.Http.Json;
using Cortexa.Application.Dtos.AI;
using Microsoft.Extensions.Logging;

namespace Cortexa.Infrastructure.External
{
    /// <summary>
    /// Typed HttpClient for communicating with the AI/ML backend service.
    /// Configured via AddHttpClient in DependencyInjection.
    /// </summary>
    public class AIHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AIHttpClient> _logger;

        public AIHttpClient(HttpClient httpClient, ILogger<AIHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Sends patient data to the AI backend and returns a risk score.
        /// </summary>
        public async Task<float> GetRiskScoreAsync(string patientId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Requesting risk score for Patient {PatientId}", patientId);

            try
            {
                var response = await _httpClient.GetAsync($"/api/risk-score/{patientId}", cancellationToken);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<RiskScoreResponse>(cancellationToken: cancellationToken);
                return result?.Score ?? 0f;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to get risk score for Patient {PatientId}", patientId);
                return 0f;
            }
        }

        /// <summary>
        /// Sends a RAG query to the AI backend.
        /// </summary>
        public async Task<RagQueryDto?> SendRagQueryAsync(RagQueryDto query, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending RAG query: {QueryText}", query.QueryText);

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/rag/query", query, cancellationToken);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<RagQueryDto>(cancellationToken: cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "RAG query failed for: {QueryText}", query.QueryText);
                return null;
            }
        }

        /// <summary>
        /// Requests an alert evaluation from the AI backend.
        /// </summary>
        public async Task<AlertDto?> EvaluateAlertAsync(string admissionId, AlertDto alert, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Evaluating alert for Admission {AdmissionId}", admissionId);

            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    $"/api/alerts/evaluate/{admissionId}", alert, cancellationToken);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<AlertDto>(cancellationToken: cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Alert evaluation failed for Admission {AdmissionId}", admissionId);
                return null;
            }
        }

        // ── Internal response model ──────────────────────────────────
        private record RiskScoreResponse(float Score);
    }
}
