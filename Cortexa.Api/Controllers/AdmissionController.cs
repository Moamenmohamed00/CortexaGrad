using Cortexa.Application.Features.Admission.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
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
        //[HttpPost("patients/{patientId}")]
        //public async Task<IActionResult> Create(
        //    string patientId,
        //    CreateAdmissionCommand command)
        //{
        //    if (patientId != command.PatientId)
        //        return BadRequest("Route patientId does not match command patientId.");

        //    var admissionId = await Sender.Send(command);
        //    var admission = await Sender.Send(
        //        new GetAdmissionsByPatientIdQuery(patientId));

        //    return CreatedAtAction(
        //        nameof(GetById),
        //        new { id = admissionId },
        //        admission);
        //}

        ///// <summary>
        ///// Discharges an admission.
        ///// </summary>
        //[HttpPut("{id}/discharge")]
        //public async Task<IActionResult> Discharge(
        //    string id,
        //    DischargePatientCommand command)
        //{
        //    if (id != command.AdmissionId)
        //        return BadRequest("Route id does not match admission id.");

        //    var success = await Sender.Send(command);

        //    return success ? NoContent() : NotFound();
        //}

        /// <summary>
        /// ÌÃ·» Õ«·… «·ﬁ»Ê· ⁄‰ ÿ—Ìﬁ «·„⁄—› «·Œ«’ »Â« (Admission ID).
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id) //  „  €ÌÌ— «·„⁄«„· ≈·Ï id
        {
            //  „ «· ’ÕÌÕ: «·¬‰ Ì»ÕÀ ⁄‰ Õ«·… ﬁ»Ê· „Õœœ… Ê·Ì” ﬁ«∆„… »Õ«·«  «·„—Ì÷
            var result = await Sender.Send(new GetAdmissionByIdQuery(id));

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
