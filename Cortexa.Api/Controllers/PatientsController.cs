using Cortexa.Application.Features.Patients.Commands;
using Cortexa.Application.Features.Patients.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    public class PatientsController : ApiControllerBase
    {
        /// <summary>
        /// Creates a new patient record.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePatientCommand command)
        {
            var patientId = await Sender.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = patientId }, new { id = patientId });
        }

        /// <summary>
        /// Admits a patient (creates patient + admission in one step).
        /// </summary>
        [HttpPost("admit")]
        public async Task<IActionResult> AdmitPatient([FromBody] AdmitPatientCommand command)
        {
            var result = await Sender.Send(command);
            return CreatedAtAction(nameof(GetDetails), new { patientId = result.PatientId }, result);
        }

        /// <summary>
        /// Updates an existing patient record.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdatePatientCommand command)
        {
            if (id != command.Id)
                return BadRequest("Route id does not match command id.");

            var success = await Sender.Send(command);
            return success ? NoContent() : NotFound();
        }

        /// <summary>
        /// Discharges a patient from an admission.
        /// </summary>
        [HttpPost("discharge")]
        public async Task<IActionResult> Discharge([FromBody] DischargePatientCommand command)
        {
            var success = await Sender.Send(command);
            return success ? NoContent() : NotFound();
        }

        /// <summary>
        /// Gets a patient by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await Sender.Send(new GetPatientByIdQuery(id));
            return result is not null ? Ok(result) : NotFound();
        }

        /// <summary>
        /// Gets detailed patient information including admissions.
        /// </summary>
        [HttpGet("{patientId}/details")]
        public async Task<IActionResult> GetDetails(string patientId)
        {
            var result = await Sender.Send(new GetPatientDetailsQuery(patientId));
            return Ok(result);
        }

        /// <summary>
        /// Gets all patients.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await Sender.Send(new GetAllPatientsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Gets all admissions for a specific patient.
        /// </summary>
        [HttpGet("{patientId}/admissions")]
        public async Task<IActionResult> GetPatientAdmissions(string patientId)
        {
            var result = await Sender.Send(new GetPatientAdmissionsQuery(patientId));
            return Ok(result);
        }
    }
}
