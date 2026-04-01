using Cortexa.Application.Features.ClinicalData.Commands.AddCommands;
using Cortexa.Application.Features.ClinicalData.Commands.DeleteCommands;
using Cortexa.Application.Features.ClinicalData.Commands.UpdateCommands;
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

            return result is not null ? Ok(result) : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> Update(string admissionId, [FromBody] UpdateVitalsCommand command)
        {
            if (admissionId != command.Id)
            {
                return BadRequest("ID mismatch");
            }

            var success = await Sender.Send(command);

            if (!success) return NotFound();

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string admissionId)
        {
            var success = await Sender.Send(new DeleteVitalSignCommand(admissionId));

            if (!success) return NotFound();

            return NoContent(); // 204 No Content
        }
    }
}
