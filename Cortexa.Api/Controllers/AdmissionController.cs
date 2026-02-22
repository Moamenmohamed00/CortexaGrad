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
    }
}
