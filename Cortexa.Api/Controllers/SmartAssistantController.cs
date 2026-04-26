using Cortexa.Application.Common.Interfaces;
using Cortexa.Application.Features.SmartAssistant.Commands;
using Cortexa.Application.Features.SmartAssistant.Queries;
using Cortexa.Application.Interfaces.Services;
using Cortexa.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Api.Controllers
{

    /// <summary>
    /// Endpoints for the AI Smart Assistant features (alerts, RAG queries).
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SmartAssistantController : ApiControllerBase
    {
        private readonly IAIService _aiService;
        private readonly ICurrentUserService _currentUserService;

        public SmartAssistantController(ISender sender,IAIService aiService, ICurrentUserService currentUserService): base(sender) 
        {
            _aiService = aiService;
            _currentUserService = currentUserService;
        }

        // ── RAG Integration ────────────────────────────────────────────────

        /// <summary>
        /// Ask the AI a question. Patient clinical data is automatically fetched
        /// from the database using admissionId and sent as context to the RAG model.
        /// The chat interaction is then saved to the patient's individual AI chat history.
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

            //var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown_doctor";
            var doctorId = _currentUserService.UserId ?? "unknown_doctor";


            var command = new AskRAGQueryCommand(
                ProjectId: projectId,
                AdmissionId: admissionId,
                DoctorId: doctorId,
                QueryText: request.Text,
                Limit: request.Limit
            );

            var result = await Sender.Send(command, ct);
            return Ok(result);
        }
        
        /// <summary>
        /// Fetch paginated AI chat history for a given patient.
        /// </summary>
        [HttpGet("rag/chats/patient/{patientId}")]
        public async Task<IActionResult> GetPatientChats(string patientId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
        {
            var query = new GetRAGQueriesByPatientIdQuery(patientId, pageNumber, pageSize);
            var result = await Sender.Send(query, ct);
            
            return Ok(new {
                Chats = result.Items,
                TotalCount = result.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            });
        }

        /// <summary>
        /// Upload and index a document for a specific project workspace.
        /// </summary>
        [HttpPost("rag/upload")]
        public async Task<IActionResult> UploadDocument([FromQuery] string projectId, Microsoft.AspNetCore.Http.IFormFile file, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(projectId))
                return BadRequest("projectId is required.");
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            using var stream = file.OpenReadStream();
            var result = await _aiService.UploadAndIndexDocumentAsync(projectId, stream, file.FileName, ct);
            
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

            var result = await _aiService.GetIndexInfoAsync(projectId, ct);
            return Ok(result ?? new { message = "No index info available." });
        }

        // ── Alerts ─────────────────────────────────────────────────────────

        /// <summary>
        /// Fetch active alerts based on patient or admission filters.
        /// </summary>
        [HttpGet("alerts/active")]
        public async Task<IActionResult> GetActiveAlerts([FromQuery] string? patientId, [FromQuery] string? admissionId)
        {
            var query = new GetActiveAlertsQuery
            {
                PatientId = patientId,
                AdmissionId = admissionId
            };

            var alerts = await Sender.Send(query);
            return Ok(alerts);
        }

        /// <summary>
        /// Override a given alert.
        /// </summary>
        [HttpPost("alerts/{id}/override")]
        public async Task<IActionResult> OverrideAlert(string id, [FromBody] OverrideAlertCommand command)
        {
            if (id != command.AlertId)
            {
                return BadRequest("The alert ID in the URL does not match the ID in the request body.");
            }

            var result = await Sender.Send(command);

            if (!result)
            {
                return NotFound(new { message = "Alert not found or could not be overridden." });
            }

            return Ok(new { message = "Alert overridden successfully." });
        }
    }
}
