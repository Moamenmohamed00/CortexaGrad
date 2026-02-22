using Cortexa.Application.Features.ClinicalData.Commands;
using Cortexa.Application.Features.ClinicalData.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    public class ClinicalDataController : ApiControllerBase
    {
        /// <summary>
        /// Records vital signs for an admission.
        /// </summary>
        [HttpPost("vitals")]
        public async Task<IActionResult> RecordVitals([FromBody] RecordVitalsCommand command)
        {
            var id = await Sender.Send(command);
            return CreatedAtAction(nameof(GetVitalsHistory), new { admissionId = command.AdmissionId }, new { id });
        }

        /// <summary>
        /// Adds a nursing note for an admission.
        /// </summary>
        [HttpPost("nursing-notes")]
        public async Task<IActionResult> AddNursingNote([FromBody] AddNursingNoteCommand command)
        {
            var id = await Sender.Send(command);
            return Created($"api/clinicaldata/nursing-notes/{id}", new { id });
        }

        /// <summary>
        /// Prescribes medication for an admission.
        /// </summary>
        [HttpPost("medications")]
        public async Task<IActionResult> PrescribeMedication([FromBody] PrescribeMedicationCommand command)
        {
            var id = await Sender.Send(command);
            return Created($"api/clinicaldata/medications/{id}", new { id });
        }

        /// <summary>
        /// Adds a fluid balance entry for an admission.
        /// </summary>
        [HttpPost("fluid-balance")]
        public async Task<IActionResult> AddFluidBalance([FromBody] AddFluidBalanceCommand command)
        {
            var id = await Sender.Send(command);
            return CreatedAtAction(nameof(GetFluidBalance), new { admissionId = command.AdmissionId }, new { id });
        }

        /// <summary>
        /// Gets the vitals history for an admission.
        /// </summary>
        [HttpGet("vitals/{admissionId}")]
        public async Task<IActionResult> GetVitalsHistory(string admissionId)
        {
            var result = await Sender.Send(new GetVitalsHistoryQuery(admissionId));
            return Ok(result);
        }

        /// <summary>
        /// Gets the fluid balance records for an admission.
        /// </summary>
        [HttpGet("fluid-balance/{admissionId}")]
        public async Task<IActionResult> GetFluidBalance(string admissionId)
        {
            var result = await Sender.Send(new GetFluidBalanceQuery(admissionId));
            return Ok(result);
        }
    }
}
