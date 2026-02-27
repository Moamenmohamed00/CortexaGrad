using Cortexa.Application.Features.ClinicalData.Commands;
using Cortexa.Application.Features.ClinicalData.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    [ApiController]
    [Route("api/admissions/{admissionId}/vitals")]
    public class VitalSignsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Record(
            string admissionId,
            [FromBody] RecordVitalsCommand command)
        {
            command.AdmissionId = admissionId;

            var id = await Sender.Send(command);

            return Created($"{Request.Path}/{id}", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory(string admissionId)
        {
            var result = await Sender.Send(
                new GetVitalsHistoryQuery(admissionId));

            return Ok(result);
        }
    }
}
