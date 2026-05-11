using Cortexa.Application.Features.ClinicalData.Commands;
using Cortexa.Application.Features.ClinicalData.Commands.DeleteCommands;
using Cortexa.Application.Features.ClinicalData.Commands.UpdateCommands;
using Cortexa.Application.Features.ClinicalData.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Doctor")]
    [Route("api/admissions/{admissionId}/case-history")]
    public class CaseHistoryController(ISender sender) : ApiControllerBase(sender)
    {
        /// <summary>Adds a new medical case history entry for an admission.</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Add(
            string admissionId,
            [FromBody] AddCaseHistoryCommand command)
        {
            command.AdmissionId = admissionId;

            var id = await Sender.Send(command);

            return Created($"{Request.Path}/{id}", new { id });
        }

        /// <summary>Retrieves case history for a specific admission.</summary>
        [HttpGet]
        [Authorize(Roles = "Doctor,Nurse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string admissionId)
        {
            var result = await Sender.Send(
                new GetCaseHistoryQuery(admissionId));

            return result is not null ? Ok(result) : NotFound();
        }

        /// <summary>Updates an existing case history entry.</summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(string admissionId, [FromBody] UpdateCaseHistoryCommand command)
        {
            if (admissionId == command.Id)
            {
                return BadRequest("ID mismatch");
            }

            var success = await Sender.Send(command);

            if (!success) return NotFound();

            return NoContent(); 
        }

        /// <summary>Deletes a case history entry.</summary>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(string admissionId,string Id)
        {
            var success = await Sender.Send(new DeleteCaseHistoryCommand(admissionId,Id));

            if (!success) return NotFound();

            return NoContent(); // 204 No Content
        }
    }
}
