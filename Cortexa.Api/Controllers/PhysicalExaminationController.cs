using Cortexa.Application.Features.ClinicalData.Commands.AddCommands;
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
    [Route("api/admissions/{admissionId}/physical-examination")]
    public class PhysicalExaminationController(ISender sender) : ApiControllerBase(sender)
    {
        /// <summary>
        /// Adds a new physical examination record for a specific admission.
        /// </summary>
        /// <param name="admissionId">The unique identifier of the admission to which the physical examination will be added. Cannot be null or empty.</param>
        /// <param name="command">The command containing the details of the physical examination to add. Must include a valid admission ID.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see cref="CreatedResult"/> if the addition is successful.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(
            string admissionId,
            [FromBody] AddPhysicalExaminationCommand command)
        {
            command.AdmissionId = admissionId;

            var id = await Sender.Send(command);

            return Created($"{Request.Path}/{id}", new { id });
        }
        /// <summary>
        /// Retrieves the physical examination records for a specific admission.
        /// </summary>
        /// <param name="admissionId">The unique identifier of the admission for which to retrieve physical examination records. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the physical examination records if found; otherwise, a <see cref="NotFoundResult"/>.</returns>

        [HttpGet]
        [Authorize(Roles ="Doctor,Nurse")]
        public async Task<IActionResult> Get(string admissionId)
        {
            var result = await Sender.Send(
                new GetPhysicalExaminationQuery(admissionId));

            return result is not null ? Ok(result) : NotFound();
        }

        /// <summary>
        /// Updates the physical examination record for the specified admission.
        /// </summary>
        /// <param name="admissionId">The unique identifier of the admission whose physical examination record is to be updated. Cannot be null or
        /// empty.</param>
        /// <param name="command">The command containing the updated physical examination data. The command's Id property must match the
        /// specified admissionId.</param>
        /// <returns>An IActionResult indicating the result of the update operation: returns 204 No Content if the update is
        /// successful, 400 Bad Request if the admissionId does not match the command's Id, or 404 Not Found if the
        /// record does not exist.</returns>

        [HttpPut]
        public async Task<IActionResult> Update(string admissionId, [FromBody] UpdatePhysicalExaminationCommand command)
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
        ///     Deletes the physical examination record associated with the specified admission and examination identifiers.
        /// </summary>
        /// <param name="admissionId">The unique identifier of the admission to which the physical examination belongs. Cannot be null or empty.</param>
        /// <param name="Id">The unique identifier of the physical examination to delete. Cannot be null or empty.</param>
        /// <returns>A 204 No Content response if the deletion is successful; otherwise, a 404 Not Found response if the record
        /// does not exist.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(string admissionId, string Id)
        {
            var success = await Sender.Send(new DeletePhysicalExaminationCommand(admissionId, Id));

            if (!success) return NotFound();

            return NoContent(); // 204 No Content
        }
    }
}
