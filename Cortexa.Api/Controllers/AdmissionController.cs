using Cortexa.Application.Features.Admission.Queries;
using Cortexa.Application.Features.Admission.Commands;
using Cortexa.Application.Features.Patients.Commands;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Cortexa.Api.Controllers
{
    [ApiController]
    [Route("api/admission")]
    [Authorize]
    public class AdmissionController(ISender sender) : ApiControllerBase(sender)
    {
        /// <summary>
        /// Retrieves a list of all currently active patient admissions.
        /// </summary>
        /// <returns>A list of active admissions.</returns>
        /// <response code="200">Returns the list of active admissions successfully.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user does not have required roles (Doctor/Nurse).</response>
        [HttpGet("active")]
        [Authorize(Roles = "Doctor,Nurse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetActiveAdmissions()
        {
            var result = await Sender.Send(new GetActiveAdmissionsQuery());
            return Ok(result);
        }

     
        /// <summary>Gets admission history for a specific patient.</summary>
        /// <param name="patientId">The unique identifier of the patient.</param>
        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Doctor,Nurse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByPatientId(string patientId)
        {
            var result = await Sender.Send(new GetAdmissionsByPatientIdQuery(patientId));
            return Ok(result);
        }

        /// <summary>
        /// Admits a patient to the hospital.
        /// </summary>
        /// <param name="patientId">The unique ID of the patient to be admitted.</param>
        /// <param name="command">Admission details including room and bed information.</param>
        /// <response code="201">Patient admitted successfully.</response>
        /// <response code="400">If the input data is invalid or patientId mismatch.</response>
        [HttpPost("patients/{patientId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            string patientId,
            CreateAdmissionCommand command)
        {
            if (patientId != command.PatientId)
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
        /// Discharges a patient from the hospital.
        /// </summary>
        /// <param name="id">The Admission ID.</param>
        /// <param name="command">Discharge details.</param>
        /// <response code="204">Patient discharged successfully.</response>
        /// <response code="404">If the admission record is not found.</response>
        [HttpPut("{id}/discharge")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Discharge(
            string id,
            DischargePatientCommand command)
        {
            if (id != command.AdmissionId)
                return BadRequest("Route id does not match admission id.");

            var success = await Sender.Send(command);

            return success ? NoContent() : NotFound();
        }

  
        /// <summary>Gets details of a specific admission by its ID.</summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor,Nurse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id) //     id
        {
            //  :          
            var result = await Sender.Send(new GetAdmissionByIdQuery(id));

            return result is not null ? Ok(result) : NotFound();
        }


        /// <summary>
        /// Admits a patient to the hospital if they are in system and not already admitted.
        /// and create new patient record if not in system then admit the patient to the hospital.
        /// </summary>
        /// <param name="command">Patient admission details including room and bed information.</param>
        /// <response code="201">Patient admitted successfully.</response>
        /// <response code="400">If the input data is invalid or patientId mismatch.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("admit")]
        public async Task<IActionResult> AdmitPatient([FromBody] AdmitPatientCommand command)
        {
            var result = await Sender.Send(command);

            // ��� ���� ����� ��� Get ������ �� Controller ����ݡ ��� ����� ���
            // �� ������� Ok(result) ��� ��� �� ���� �� Location Header
            return Ok(result);

        }
    }
}