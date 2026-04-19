using Cortexa.Application.Features.SmartAssistant.Commands;
using Cortexa.Application.Features.SmartAssistant.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Cortexa.Application.Features.SmartAssistant.Commands;
using Cortexa.Application.Features.SmartAssistant.Queries;
using System.Threading.Tasks;

namespace Cortexa.Api.Controllers
{

    /// <summary>
    /// Endpoints for the AI Smart Assistant features (alerts, RAG queries).
    /// Currently a placeholder — endpoints will be enabled once the
    /// underlying MediatR commands/queries are implemented.
    /// </summary>

    [Route("api/[controller]")]
    public class SmartAssistantController(ISender sender, Cortexa.Application.Interfaces.Services.IAIService aiService) : ApiControllerBase(sender)
    {
        // ── RAG Integration ────────────────────────────────────────────────

        /// <summary>
        /// Ask the AI a question. Patient clinical data is automatically fetched
        /// from the database using admissionId and sent as context to the RAG model.
        /// </summary>
        [HttpPost("rag/ask")]
        public async Task<IActionResult> AskQuestion(
            [FromQuery] string projectId,
            [FromQuery] string admissionId,
            [FromBody] Cortexa.Application.Dtos.AI.RagSearchRequest request,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(projectId))
                return BadRequest("projectId is required.");
            if (string.IsNullOrWhiteSpace(admissionId))
                return BadRequest("admissionId is required.");

            var result = await aiService.AskQuestionAsync(projectId, admissionId, request.Text, request.Limit, ct);
            return Ok(result);
        }

        /// <summary>
        /// Upload and index a document for a specific project workspace.
        /// </summary>
        [HttpPost("rag/upload")]
        public async Task<IActionResult> UploadDocument([FromQuery] string projectId, IFormFile file, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(projectId))
                return BadRequest("projectId is required.");
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            using var stream = file.OpenReadStream();
            var result = await aiService.UploadAndIndexDocumentAsync(projectId, stream, file.FileName, ct);
            
            if (!result.Success)
                return StatusCode(500, result);

            return Ok(result);
        }

        /// <summary>
        /// Get the vector index metadata for a specific project.
        /// </summary>
        [HttpGet("rag/info")]
        public async Task<IActionResult> GetIndexInfo([FromQuery] string projectId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(projectId))
                return BadRequest("projectId is required.");

            var result = await aiService.GetIndexInfoAsync(projectId, ct);
            return Ok(result ?? new { message = "No index info available." });
        }

        // ── Alerts ─────────────────────────────────────────────────────────

        /// <summary>
        /// جلب التنبيهات النشطة بناءً على المريض أو الدخول (Admission)
        /// </summary>
        [HttpGet("alerts/active")]
        public async Task<IActionResult> GetActiveAlerts([FromQuery] string? patientId, [FromQuery] string? admissionId)
        {
            // إنشاء الـ Query بناءً على المعاملات القادمة من الرابط
            var query = new GetActiveAlertsQuery
            {
                PatientId = patientId,
                AdmissionId = admissionId
            };

            var alerts = await Sender.Send(query);
            return Ok(alerts);
        }

        /// <summary>
        /// إلغاء تنبيه وتجاوزه من قِبل الطبيب
        /// </summary>
        [HttpPost("alerts/{id}/override")]
        public async Task<IActionResult> OverrideAlert(string id, [FromBody] OverrideAlertCommand command)
        {
            // تأمين إضافي للتأكد من أن الـ ID في الرابط يطابق الـ ID في جسم الطلب
            if (id != command.AlertId)
            {
                return BadRequest("The alert ID in the URL does not match the ID in the request body.");
            }

            var result = await Sender.Send(command);

            if (!result)
            {
                // إذا رجع false، فهذا يعني أن التنبيه غير موجود في قاعدة البيانات
                return NotFound(new { message = "Alert not found or could not be overridden." });
            }

            // إرجاع 200 OK للإشارة لنجاح العملية
            return Ok(new { message = "Alert overridden successfully." });
        }
    }
}