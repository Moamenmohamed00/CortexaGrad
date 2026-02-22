using Cortexa.Application.Features.Admission.Commands;
using Cortexa.Application.Features.Admission.Queries;
using Cortexa.Application.Features.Patients.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    [Route("api/[controller]")]

    public class AdmissionController : ApiControllerBase
    {
        /// <summary>
        /// Gets all currently active admissions.
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveAdmissions()
        {
            var result = await Sender.Send(new GetActiveAdmissionsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Gets all admissions for a specific patient.
        /// </summary>
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatientId(string patientId)
        {
            var result = await Sender.Send(new GetAdmissionsByPatientIdQuery(patientId));
            return Ok(result);
        }

        /// <summary>
        /// Creates a new admission (admit patient).
        /// </summary>
        [HttpPost("patients/{patientId}")]
        public async Task<IActionResult> Create(
            string patientId,
            CreateAdmissionCommand command)
        {
            if (patientId != patientId)
                return BadRequest("Route patientId does not match command patientId.");

            var admissionId = await Sender.Send(command);
            var admission = await Sender.Send(
                new GetAdmissionsByPatientIdQuery(patientId));

            return CreatedAtAction(
                nameof(GetById),
                new { id = admissionId },
                admission);
        }

        /// <summary>
        /// Discharges an admission.
        /// </summary>
        [HttpPut("{id}/discharge")]
        public async Task<IActionResult> Discharge(
            string id,
            DischargePatientCommand command)
        {
            if (id != command.AdmissionId)
                return BadRequest("Route id does not match admission id.");

            var success = await Sender.Send(command);

            return success ? NoContent() : NotFound();
        }

        /// <summary>
        /// Gets admission by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string patientId)
        {
            var result = await Sender.Send(new GetAdmissionsByPatientIdQuery(patientId));

            return result is not null ? Ok(result) : NotFound();
        }
        /// <summary>
        /// Gets all admissions with pagination.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetActiveAdmissionsQuery query)
        {
            var result = await Sender.Send(query);
            return Ok(result);
        }
    }
}

