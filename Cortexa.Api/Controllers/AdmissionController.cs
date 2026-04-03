using Cortexa.Application.Features.Admission.Queries;
using Cortexa.Application.Features.Admission.Commands;
using Cortexa.Application.Features.Patients.Commands;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Cortexa.Api.Controllers
{
    [ApiController]
    [Route("api/admissions")]
    [Authorize]
    public class AdmissionController(ISender sender) : ApiControllerBase(sender)
    {
        /// <summary>
        /// Gets all currently active admissions.
        /// </summary>
        [HttpGet("active")]
        [Authorize(Roles ="Doctor,Nurse")]
        public async Task<IActionResult> GetActiveAdmissions()
        {
            var result = await Sender.Send(new GetActiveAdmissionsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Gets all admissions for a specific patient.
        /// </summary>
        [HttpGet("patient/{patientId}")]
        [Authorize(Roles ="Doctor,Nurse")]
        public async Task<IActionResult> GetByPatientId(string patientId)
        {
            var result = await Sender.Send(new GetAdmissionsByPatientIdQuery(patientId));
            return Ok(result);
        }

        /// <summary>
        /// Creates a new admission(admit patient).
        /// </summary>
        [HttpPost("patients/{patientId}")]
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
        ///        (Admission ID).
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles ="Doctor,Nurse")]
        public async Task<IActionResult> GetById(string id) //     id
        {
            //  :          
            var result = await Sender.Send(new GetAdmissionByIdQuery(id));

            return result is not null ? Ok(result) : NotFound();
        }


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