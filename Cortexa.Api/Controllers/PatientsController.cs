using Cortexa.Application.Features.Patients.Commands;
using Cortexa.Application.Features.Patients.Queries;
using Cortexa.Application.Features.Rooms.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Cortexa.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/Patients")]
    public class PatientsController(ISender sender) : ApiControllerBase(sender)
    {
        /// <summary>
        /// Registers a new patient.
        /// </summary>
        /// <response code="201">Patient created successfully.</response>
        /// <response code="400">Invalid patient data.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreatePatientCommand command)
        {
            var patient = await Sender.Send(command);

            return Ok(patient);
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
        /// Gets detailed information for a specific patient.
        /// </summary>
        /// <param name="id">The patient's unique identifier.</param>
        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor,Nurse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [Authorize(Roles ="Doctor,Nurse")]
        public async Task<IActionResult> GetDetails(string id)
        {
            var result = await Sender.Send(new GetPatientDetailsQuery(id));

            return result is not null
                ? Ok(result)
                : NotFound();
        }

        /// <summary>
        /// Retrieves a paginated list of all patients.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Doctor,Nurse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            var result = await Sender.Send(new GetAllPatientsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Gets all admissions for a specific patient.
        /// </summary>
        [HttpGet("{id}/admissions")]
        [Authorize(Roles ="Doctor,Nurse")]
        public async Task<IActionResult> GetPatientAdmissions(string id)
        {
            var result = await Sender.Send(new GetPatientAdmissionsQuery(id));
            return Ok(result);
        }
    }

}