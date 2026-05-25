using Cortexa.Api.Extensions;
using Cortexa.Application.Features.Diagnostics.Commands;
using Cortexa.Application.Features.Diagnostics.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/Diagnostics")]
    public class DiagnosticsController(ISender sender) : ApiControllerBase(sender)
    {
        /// <summary>
        /// Creates a lab order for an admission.
        /// </summary>
        [HttpPost("lab-orders")]
        public async Task<IActionResult> CreateLabOrder([FromBody] CreateLabOrderCommand command)
        {
            var id = await Sender.Send(command);
            return CreatedAtAction(nameof(GetLabOrders), new { admissionId = command.AdmissionId }, new { id });
        }

        /// <summary>
        /// Adds a lab result to an existing lab order.
        /// </summary>
        [HttpPost("lab-results")]
        public async Task<IActionResult> AddLabResult([FromBody] AddLabResultCommand command)
        {
            var id = await Sender.Send(command);
            return Created($"api/diagnostics/lab-results/{id}", new { id });
        }

        /// <summary>
        /// Uploads an imaging study for an admission.
        /// </summary>
        [HttpPost("imaging")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImaging([FromForm] UploadImagingRequest request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest("File is empty or not provided.");
            }

            using var ms = new MemoryStream();
            await request.File.CopyToAsync(ms);

            var command = new UploadImagingCommand
            {
                Content = ms.ToArray(),
                FileName = request.File.FileName,
                AdmissionId = request.AdmissionId,
                Type = request.Type,
                Findings = request.Findings,
                Date = request.Date,
                DoctorId = request.DoctorId
            };

            var result = await Sender.Send(command);

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        /// <summary>
        /// Gets all lab orders for an admission.
        /// </summary>
        [HttpGet("lab-orders/{admissionId}")]
        [Authorize(Roles ="Doctor,Nurse")]
        public async Task<IActionResult> GetLabOrders(string admissionId)
        {
            var result = await Sender.Send(new GetLabOrdersQuery(admissionId));
            return Ok(result);
        }

        /// <summary>
        /// Gets all lab results for a specific lab order.
        /// </summary>
        [HttpGet("lab-results/{orderId}")]
        [Authorize(Roles ="Doctor,Nurse")]
        public async Task<IActionResult> GetLabResults(string orderId)
        {
            var result = await Sender.Send(new GetLabResultsQuery(orderId));
            return Ok(result);
        }

        /// <summary>
        /// Gets all imaging studies for an admission.
        /// </summary>
        [HttpGet("imaging/{admissionId}")]
        [Authorize(Roles ="Doctor,Nurse")]

        public async Task<IActionResult> GetImagingStudies(string admissionId)
        {
            var result = await Sender.Send(new GetImagingStudiesQuery(admissionId));
            return Ok(result);
        }
    }
}
