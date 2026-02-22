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
        public async Task<IActionResult> Create(CreatePatientCommand command)
        {
            var patientId = await Sender.Send(command);

            // Fetch full resource for response (best practice)
            var patient = await Sender.Send(new GetPatientByIdQuery(patientId));

            return CreatedAtAction(
                nameof(GetById),
                new { id = patientId },
                patient
            );
        }

        /// <summary>
        /// Updates an existing patient record.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdatePatientCommand command)
        {
            if (id != command.Id)
                return BadRequest("Route id does not match command id.");

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

            return result is not null
                ? Ok(result)
                : NotFound();
        }

        /// <summary>
        /// Gets detailed patient information including admissions.
        /// </summary>
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetDetails(string id)
        {
            var result = await Sender.Send(new GetPatientDetailsQuery(id));

            return result is not null
                ? Ok(result)
                : NotFound();
        }

        /// <summary>
        /// Gets all patients with pagination support.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllPatientsQuery query)
        {
            var result = await Sender.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Gets all admissions for a specific patient.
        /// </summary>
        [HttpGet("{id}/admissions")]
        public async Task<IActionResult> GetPatientAdmissions(string id)
        {
            var result = await Sender.Send(new GetPatientAdmissionsQuery(id));
            return Ok(result);
        }
    }
}