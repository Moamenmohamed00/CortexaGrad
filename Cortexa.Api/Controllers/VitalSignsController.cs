using Cortexa.Application.Features.ClinicalData.Commands.AddCommands;
using Cortexa.Application.Features.ClinicalData.Commands.DeleteCommands;
using Cortexa.Application.Features.ClinicalData.Commands.UpdateCommands;
using Cortexa.Application.Features.ClinicalData.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Cortexa.Api.Controllers
{
    [ApiController]
    [Route("api/admissions/{admissionId}/vitals")]
    [Authorize]
    public class VitalSignsController(ISender sender) : ApiControllerBase(sender)
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
        [Authorize(Roles ="Doctor,Nurse")]
        public async Task<IActionResult> GetHistory(string admissionId)
        {
            var result = await Sender.Send(
                new GetVitalsHistoryQuery(admissionId));

            return result is not null ? Ok(result) : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> Update(string admissionId, [FromBody] UpdateVitalsCommand command)
        {
            if (admissionId == command.Id)
            {
                return BadRequest("ID mismatch");
            }

            var success = await Sender.Send(command);

            if (!success) return NotFound();

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string admissionId, string Id)
        {
            var success = await Sender.Send(new DeleteVitalSignCommand(admissionId, Id));

            if (!success) return NotFound();

            return NoContent(); // 204 No Content
        }
    }
}
