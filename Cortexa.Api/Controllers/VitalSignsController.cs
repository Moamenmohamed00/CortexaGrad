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
        /// <summary>
        /// Records new vital signs for an admitted patient.
        /// </summary>
        /// <param name="admissionId">The unique identifier of the admission for which to record vital signs. Cannot be null or empty.</param>
        /// <param name="command">The command containing the vital signs data to record. Must include a valid admission ID.</param>
        /// <response code="201">Vitals recorded and NEWS score calculated.</response>
        /// <response code="400">Invalid vitals data.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Record(
            string admissionId,
            [FromBody] RecordVitalsCommand command)
        {
            command.AdmissionId = admissionId;

            var id = await Sender.Send(command);

            return Created($"{Request.Path}/{id}", new { id });
        }

        /// <summary>
        /// Retrieves the history of vital signs for a patient.
        /// </summary>
        /// <param name="admissionId">The unique identifier of the admission for which to retrieve vital signs history. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the vital signs history if found; otherwise, a <see cref="NotFoundResult"/>.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles ="Doctor,Nurse")]
        public async Task<IActionResult> GetHistory(string admissionId)
        {
            var result = await Sender.Send(
                new GetVitalsHistoryQuery(admissionId));

            return result is not null ? Ok(result) : NotFound();
        }
        /// <summary>
        /// Updates the vital signs for a patient.
        /// </summary>
        /// <param name="admissionId">The unique identifier of the admission for which to update vital signs. Cannot be null or empty.</param>
        /// <param name="command">The command containing the updated vital signs data. The command's Id property must match the specified admissionId.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation: returns 204 No Content if the update is successful, 400 Bad Request if the admissionId does not match the command's Id, or 404 Not Found if the record does not exist.</returns>
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
        /// <summary>
        /// Deletes the specified vital sign record associated with the given admission identifier.
        /// </summary>
        /// <param name="admissionId">The unique identifier of the admission to which the vital sign record belongs. Cannot be null or empty.</param>
        /// <param name="Id">The unique identifier of the vital sign record to delete. Cannot be null or empty.</param>
        /// <returns>A 204 No Content response if the deletion is successful; otherwise, a 404 Not Found response if the record
        /// does not exist.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(string admissionId, string Id)
        {
            var success = await Sender.Send(new DeleteVitalSignCommand(admissionId, Id));

            if (!success) return NotFound();

            return NoContent(); // 204 No Content
        }
    }
}
