using System.Text;
using Cortexa.Application.Dtos.AI;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cortexa.Infrastructure.External
{
    /// <summary>
    /// Implements IAIService by:
    /// 1. Fetching all clinical data for the admission from the database
    /// 2. Building a patient context string
    /// 3. Sending question + context to the RAG model
    /// </summary>
    public class PythonRAGService : IAIService
    {
        private readonly AIHttpClient _aiClient;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PythonRAGService> _logger;
        private readonly string _filesBaseUrl;

        public PythonRAGService(
            AIHttpClient aiClient,
            IUnitOfWork unitOfWork,
            ILogger<PythonRAGService> logger,
            IConfiguration configuration)
        {
            _aiClient = aiClient;
            _unitOfWork = unitOfWork;
            _logger = logger;
            // Falls back to HuggingFace space URL if not configured
            _filesBaseUrl = configuration["AIService:FilesBaseUrl"]
                ?? "https://huggingface.co/spaces/M0amenmohamed/rag/resolve/main";
        }

        // ── Ask Question (auto-fetches patient data) ───────────────────────
        public async Task<RagAnswerResponse> AskQuestionAsync(
            string projectId,
            string admissionId,
            string question,
            int limit = 5,
            CancellationToken ct = default)
        {
            // 1. Build patient context from the database
            var context = await BuildPatientContextAsync(admissionId);

            // 2. Combine question + context into a single prompt
            var enrichedQuestion = string.IsNullOrWhiteSpace(context)
                ? question
                : $"{question}\n\n--- Patient Clinical Data ---\n{context}";

            _logger.LogInformation(
                "RAG ask — project: {Project}, admission: {Adm}, question length: {Len}",
                projectId, admissionId, enrichedQuestion.Length);

            // 3. Send to RAG model
            var result = await _aiClient.AskAsync(projectId, enrichedQuestion, limit, ct);

            if (result is null)
            {
                _logger.LogWarning("RAG returned no answer for project {Id}", projectId);
                return new RagAnswerResponse
                {
                    Answer = "AI assistant unavailable.",
                    Sources = []
                };
            }

            // ── Compute page-level links for each retrieved source ─────────
            // The FastAPI returns source_file_name in two formats:
            //   a) Full Linux path: /app/src/assets/files/1/file.pdf  → strip "/app"
            //   b) Bare filename:   file.pdf                           → prepend /src/assets/files/{projectId}/
            foreach (var source in result.RetrievedSources)
            {
                var fileName = source.SourceFileName;
                string relativePath;

                if (fileName.StartsWith("/app/", StringComparison.OrdinalIgnoreCase))
                    relativePath = fileName[4..];          // "/app" is 4 chars
                else if (fileName.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                    relativePath = fileName;               // already an absolute path without /app
                else
                    relativePath = $"/src/assets/files/{projectId}/{fileName}";

                source.Link = source.PageNumber.HasValue
                    ? $"{_filesBaseUrl}{relativePath}#page={source.PageNumber}"
                    : $"{_filesBaseUrl}{relativePath}";
            }

            return result;
        }

        // ── Build Patient Context ──────────────────────────────────────────
        private async Task<string> BuildPatientContextAsync(string admissionId)
        {
            var sb = new StringBuilder();

            try
            {
                // Case History
                var caseHistories = await _unitOfWork.CaseHistories.GetByAdmissionIdAsync(admissionId);
                foreach (var ch in caseHistories)
                {
                    sb.AppendLine($"[Case History]");
                    sb.AppendLine($"  Complaint: {ch.Complaint}");
                    sb.AppendLine($"  Present Illness: {ch.PresentIllness}");
                    if (!string.IsNullOrEmpty(ch.ChronicDisease)) sb.AppendLine($"  Chronic Disease: {ch.ChronicDisease}");
                    if (!string.IsNullOrEmpty(ch.GeneticDisease)) sb.AppendLine($"  Genetic Disease: {ch.GeneticDisease}");
                    if (!string.IsNullOrEmpty(ch.ClinicalNotes)) sb.AppendLine($"  Clinical Notes: {ch.ClinicalNotes}");
                    if (!string.IsNullOrEmpty(ch.SpecialHabits)) sb.AppendLine($"  Special Habits: {ch.SpecialHabits}");
                    sb.AppendLine();
                }

                // Latest Vital Signs
                var vitals = await _unitOfWork.VitalSigns.GetByAdmissionIdAsync(admissionId);
                var latestVitals = vitals.OrderByDescending(v => v.RecordedAt).FirstOrDefault();
                if (latestVitals is not null)
                {
                    sb.AppendLine($"[Latest Vital Signs — {latestVitals.RecordedAt:g}]");
                    sb.AppendLine($"  Temperature: {latestVitals.Temperature}°C");
                    sb.AppendLine($"  Heart Rate: {latestVitals.HeartRate} bpm");
                    sb.AppendLine($"  Respiratory Rate: {latestVitals.RespRate} /min");
                    sb.AppendLine($"  Blood Pressure: {latestVitals.BpSystolic}/{latestVitals.BpDiastolic} mmHg");
                    sb.AppendLine($"  SpO2: {latestVitals.PulseOxy}%");
                    sb.AppendLine($"  GCS: E{latestVitals.GcsEye} V{latestVitals.GcsVerbal} M{latestVitals.GcsMotor} = {latestVitals.GcsTotal}");
                    sb.AppendLine($"  NEWS Score: {latestVitals.NewsScore} ({latestVitals.NewsRiskLevel})");
                    sb.AppendLine($"  Consciousness: {latestVitals.ConsciousnessLevel}");
                    sb.AppendLine();
                }

                // Current Medications
                var medications = await _unitOfWork.Medications.GetByAdmissionIdAsync(admissionId);
                if (medications.Any())
                {
                    sb.AppendLine("[Active Medications]");
                    foreach (var m in medications)
                    {
                        sb.AppendLine($"  - {m.DrugName} {m.Dose} {m.DoseUnit}, {m.Frequency}x/day via {m.Route}");
                    }
                    sb.AppendLine();
                }

                // Nursing Notes (latest 3)
                var notes = await _unitOfWork.NursingNotes.GetByAdmissionIdAsync(admissionId);
                var recentNotes = notes.OrderByDescending(n => n.NoteDateTime).Take(3);
                if (recentNotes.Any())
                {
                    sb.AppendLine("[Recent Nursing Notes]");
                    foreach (var n in recentNotes)
                    {
                        sb.AppendLine($"  [{n.NoteDateTime:g}] {n.NoteText}");
                    }
                    sb.AppendLine();
                }

                // Fluid Balance
                var fluids = await _unitOfWork.FluidBalances.GetByAdmissionIdAsync(admissionId);
                if (fluids.Any())
                {
                    sb.AppendLine("[Fluid Balance Records]");
                    foreach (var f in fluids.OrderByDescending(f => f.RecordedAt).Take(5))
                    {
                        sb.AppendLine($"  [{f.RecordedAt:g}] {f.Category}: {f.Type} — {f.Amount_ML} mL");
                    }
                    sb.AppendLine();
                }

                // Interventions
                var interventions = await _unitOfWork.InterventionProcedures.GetByAdmissionIdAsync(admissionId);
                if (interventions.Any())
                {
                    sb.AppendLine("[Interventions/Procedures]");
                    foreach (var i in interventions)
                    {
                        sb.AppendLine($"  - {i.Type} (Size: {i.Size}), Inserted: {i.InsertionDate:g}");
                    }
                    sb.AppendLine();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to build full context for admission {Id}", admissionId);
            }

            return sb.ToString();
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

        // ── Legacy ─────────────────────────────────────────────────────────
        public Task<float> GenerateRiskScoreAsync(string patientId) => Task.FromResult(0f);
        public Task<AlertDto> GenerateAlertAsync(string admissionId, AlertDto alert) => Task.FromResult(alert);

        public async Task<RagQueryDto> ProcessRagQueryAsync(RagQueryDto query)
        {
            var projectId = query.PatientId ?? "1";
            var result = await AskQuestionAsync(projectId, projectId, query.QueryText, 5);
            return query with
            {
                GeneratedResponse = result.Answer ?? "No response",
                ScoreTrust = 0.9f,
                QueryDateTime = DateTime.UtcNow
            };
        }
    }
}
