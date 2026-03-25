using Cortexa.Application.Dtos.AI;
using Cortexa.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Cortexa.Infrastructure.External
{
    // Cortexa.Infrastructure/External/PythonRAGService.cs
    public class PythonRAGService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly AIHttpClient _aiClient;
        private readonly ILogger<PythonRAGService> _logger;


        public PythonRAGService(HttpClient httpClient, ILogger<PythonRAGService> logger, AIHttpClient aiClient)
        {
            _httpClient = httpClient;
            _logger = logger;
            _aiClient=aiClient;
        }
        public async Task<float> GenerateRiskScoreAsync(string patientId)
        {
            // «· Ê«’· „⁄ «·„ÊœÌ· «·Œ«’ »«· ‰»ƒ »„Œ«ÿ— «·„—Ì÷
            return await _aiClient.GetRiskScoreAsync(patientId);
        }

        public async Task<RagQueryDto> ProcessRagQueryAsync(RagQueryDto query)
        {
            // ≈—”«· ”ƒ«· «·ÿ»Ì» ≈·Ï „Õ—ﬂ «·‹ RAG ··Õ’Ê· ⁄·Ï ≈Ã«»… „œ⁄Ê„… »«·„—«Ã⁄ «·ÿ»Ì…
            var result = await _aiClient.SendRagQueryAsync(query);
            return result ?? query;
        }

        public async Task<AlertDto> GenerateAlertAsync(string admissionId, AlertDto alert)
        {
            //  ﬁÌÌ„ «·⁄·«„«  «·ÕÌÊÌ… Ê≈’œ«—  ‰»ÌÂ«  –ﬂÌ…
            var result = await _aiClient.EvaluateAlertAsync(admissionId, alert);
            return result ?? alert;
        }
        public async Task<string> GetAIAssistanceAsync(string query, string context)
        {
            var requestBody = new { query = query, context = context };
            var response = await _httpClient.PostAsJsonAsync("api/rag/ask", requestBody);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<RagQueryDto>();
                return result?.GeneratedResponse ?? "No response from AI.";
            }

            _logger.LogError("AI Service failed with status {Status}", response.StatusCode);
            return "AI Service Unavailable.";
        }
    }
}
