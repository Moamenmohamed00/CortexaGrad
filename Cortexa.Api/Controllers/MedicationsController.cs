using Cortexa.Application.Features.ClinicalData.Commands.AddCommands;
using Cortexa.Application.Features.ClinicalData.Commands.DeleteCommands;
using Cortexa.Application.Features.ClinicalData.Commands.UpdateCommands;
using Cortexa.Application.Features.ClinicalData.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cortexa.Domain.Constants;
namespace Cortexa.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = $"{AppRoles.Doctor},{AppRoles.Nurse},{AppRoles.Admin}")]
    [Route("api/admissions/{admissionId}/medications")]
    public class MedicationsController(ISender sender) : ApiControllerBase(sender)
    {
        /// <summary>Prescribes a new medication for a patient.</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Prescribe(
            string admissionId,
            [FromBody] PrescribeMedicationCommand command)
        {
            command.AdmissionId = admissionId;

            var id = await Sender.Send(command);

            return Created($"{Request.Path}/{id}", new { id });
        }


        /// <summary>Retrieves all prescribed medications for an admission.</summary>
        [HttpGet]
        public async Task<IActionResult> Get(string admissionId)
        {
            var result = await Sender.Send(
                new GetMedicationsQuery(admissionId));

            return result is not null ? Ok(result) : NotFound();
        }

        /// <summary>Updates prescription details.</summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(string admissionId, [FromBody] UpdateMedicationCommand command)
        {
            if (admissionId == command.Id)
            {
                return BadRequest("ID mismatch");
            }

            var success = await Sender.Send(command);

            if (!success) return NotFound();

            return NoContent();
        }

        /// <summary>Deletes a medication entry.</summary>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(string admissionId, string Id)
        {
            var success = await Sender.Send(new DeleteMedicationCommand(admissionId, Id));

            if (!success) return NotFound();

            return NoContent(); // 204 No Content
        }

        /// <summary>Searches the Egyptian drug database.</summary>
        [HttpGet("~/api/medications/search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchDrugs([FromQuery] string? term)
        {
            var result = await Sender.Send(new SearchEgyptianDrugsQuery(term));
            
            return Ok(result);
        }
    }
}
