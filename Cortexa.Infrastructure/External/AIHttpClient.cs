using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using Cortexa.Application.Dtos.AI;
using Microsoft.Extensions.Logging;

namespace Cortexa.Infrastructure.External
{
    /// <summary>
    /// Typed HttpClient for the RAG model hosted at https://m0amenmohamed-rag.hf.space.
    /// The project_id maps to admissionId — each admission has its own document workspace.
    /// </summary>
    public class AIHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AIHttpClient> _logger;

        private static readonly JsonSerializerOptions _jsonOpts = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public AIHttpClient(HttpClient httpClient, ILogger<AIHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        // ── Answer ─────────────────────────────────────────────────────────
        /// <summary>
        /// POST /api/v1/nlp/index/answer/{project_id}
        /// Returns a grounded AI answer from the indexed documents.
        /// </summary>
        public async Task<RagAnswerResponse?> AskAsync(
            string projectId,
            string question,
            int limit = 5,
            CancellationToken ct = default)
        {
            _logger.LogInformation("RAG ask — project: {Project}, question: {Q}", projectId, question);
            try
            {
                var body = new RagSearchRequest { Text = question, Limit = limit };
                var response = await _httpClient.PostAsJsonAsync(
                    $"/api/v1/nlp/index/answer/{projectId}", body, ct);

                response.EnsureSuccessStatusCode();

                var raw = await response.Content.ReadAsStringAsync(ct);
                _logger.LogDebug("RAG answer raw: {Raw}", raw);

                // Parse flexibly — the API returns a dynamic object
                using var doc = JsonDocument.Parse(raw);
                var root = doc.RootElement;

                var answer = root.TryGetProperty("answer", out var ans)
                    ? ans.GetString()
                    : root.TryGetProperty("result", out var res)
                        ? res.GetString()
                        : raw;

                var sources = new List<string>();
                if (root.TryGetProperty("sources", out var srcArr) && srcArr.ValueKind == JsonValueKind.Array)
                    foreach (var s in srcArr.EnumerateArray())
                        if (s.GetString() is { } src) sources.Add(src);

                return new RagAnswerResponse { Answer = answer, Sources = sources };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RAG ask failed for project {Project}", projectId);
                return null;
            }
        }

        // ── Search ─────────────────────────────────────────────────────────
        /// <summary>
        /// POST /api/v1/nlp/index/search/{project_id}
        /// Returns raw matching chunks (no AI generation).
        /// </summary>
        public async Task<string?> SearchAsync(
            string projectId,
            string text,
            int limit = 5,
            CancellationToken ct = default)
        {
            _logger.LogInformation("RAG search — project: {Project}", projectId);
            try
            {
                var body = new RagSearchRequest { Text = text, Limit = limit };
                var response = await _httpClient.PostAsJsonAsync(
                    $"/api/v1/nlp/index/search/{projectId}", body, ct);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RAG search failed for project {Project}", projectId);
                return null;
            }
        }

        // ── Upload ─────────────────────────────────────────────────────────
        /// <summary>
        /// POST /api/v1/data/upload/{project_id}
        /// Uploads a file (multipart/form-data). Returns the file_id needed for processing.
        /// </summary>
        public async Task<string?> UploadFileAsync(
            string projectId,
            Stream fileStream,
            string fileName,
            CancellationToken ct = default)
        {
            _logger.LogInformation("RAG upload — project: {Project}, file: {File}", projectId, fileName);
            try
            {
                using var content = new MultipartFormDataContent();
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(fileContent, "file", fileName);

                var response = await _httpClient.PostAsync(
                    $"/api/v1/data/upload/{projectId}", content, ct);
                response.EnsureSuccessStatusCode();

                var raw = await response.Content.ReadAsStringAsync(ct);
                _logger.LogDebug("RAG upload raw response: {Raw}", raw);

                using var doc = JsonDocument.Parse(raw);
                var root = doc.RootElement;

                // The API may return file_id directly or nested
                if (root.TryGetProperty("file_id", out var fid)) return fid.GetString();
                if (root.TryGetProperty("id", out var id)) return id.GetString();

                return raw; // fallback: return raw response as file_id
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RAG upload failed for project {Project}", projectId);
                return null;
            }
        }

        // ── Process ────────────────────────────────────────────────────────
        /// <summary>
        /// POST /api/v1/data/process/{project_id}
        /// Chunks the uploaded file. Must be called after upload.
        /// </summary>
        public async Task<bool> ProcessFileAsync(
            string projectId,
            string fileId,
            int chunkSize = 600,
            int overlapSize = 50,
            CancellationToken ct = default)
        {
            _logger.LogInformation("RAG process — project: {Project}, fileId: {FileId}", projectId, fileId);
            try
            {
                var body = new
                {
                    file_id = fileId,
                    chunk_size = chunkSize,
                    overlap_size = overlapSize,
                    do_reset = 0
                };
                var response = await _httpClient.PostAsJsonAsync(
                    $"/api/v1/data/process/{projectId}", body, ct);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RAG process failed for project {Project}", projectId);
                return false;
            }
        }

        // ── Push to Index ──────────────────────────────────────────────────
        /// <summary>
        /// POST /api/v1/nlp/index/push/{project_id}
        /// Pushes processed chunks into the vector index.
        /// </summary>
        public async Task<bool> PushToIndexAsync(
            string projectId,
            bool doReset = false,
            CancellationToken ct = default)
        {
            _logger.LogInformation("RAG push index — project: {Project}", projectId);
            try
            {
                var body = new { do_reset = doReset };
                var response = await _httpClient.PostAsJsonAsync(
                    $"/api/v1/nlp/index/push/{projectId}", body, ct);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RAG push failed for project {Project}", projectId);
                return false;
            }
        }

        // ── Index Info ─────────────────────────────────────────────────────
        /// <summary>
        /// GET /api/v1/nlp/index/info/{project_id}
        /// Returns metadata about the vector index for this workspace.
        /// </summary>
        public async Task<string?> GetIndexInfoAsync(
            string projectId,
            CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"/api/v1/nlp/index/info/{projectId}", ct);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RAG index info failed for project {Project}", projectId);
                return null;
            }
        }
    }
}
