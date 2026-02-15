using Cortexa.Application.Features.Diagnostics.Commands;
using Cortexa.Application.Features.Diagnostics.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    public class DiagnosticsController : ApiControllerBase
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
        public async Task<IActionResult> UploadImaging([FromBody] UploadImagingCommand command)
        {
            var id = await Sender.Send(command);
            return CreatedAtAction(nameof(GetImagingStudies), new { admissionId = command.AdmissionId }, new { id });
        }

        /// <summary>
        /// Gets all lab orders for an admission.
        /// </summary>
        [HttpGet("lab-orders/{admissionId}")]
        public async Task<IActionResult> GetLabOrders(string admissionId)
        {
            var result = await Sender.Send(new GetLabOrdersQuery(admissionId));
            return Ok(result);
        }

        /// <summary>
        /// Gets all lab results for a specific lab order.
        /// </summary>
        [HttpGet("lab-results/{orderId}")]
        public async Task<IActionResult> GetLabResults(string orderId)
        {
            var result = await Sender.Send(new GetLabResultsQuery(orderId));
            return Ok(result);
        }

        /// <summary>
        /// Gets all imaging studies for an admission.
        /// </summary>
        [HttpGet("imaging/{admissionId}")]
        public async Task<IActionResult> GetImagingStudies(string admissionId)
        {
            var result = await Sender.Send(new GetImagingStudiesQuery(admissionId));
            return Ok(result);
        }
    }
}
