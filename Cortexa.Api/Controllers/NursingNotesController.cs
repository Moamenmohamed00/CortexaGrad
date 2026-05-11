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
    [Route("api/admissions/{admissionId}/nursing-notes")]
    public class NursingNotesController(ISender sender) : ApiControllerBase(sender)
    {
        /// <summary>
        /// Adds a new nursing note to the specified admission.
        /// </summary>
        /// <param name="admissionId">The unique identifier of the admission to which the nursing note will be added. Cannot be null or empty.</param>
        /// <param name="command">The command containing the details of the nursing note to add. Must include a valid admission ID.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see cref="CreatedResult"/> if the addition is successful.</returns>
        [HttpPost]
        [Authorize(Roles = "Nurse")]
        public async Task<IActionResult> Add(
            string admissionId,
            [FromBody] AddNursingNoteCommand command)
        {
            command.AdmissionId = admissionId;

            var id = await Sender.Send(command);

            return Created($"{Request.Path}/{id}", new { id });
        }
        /// <summary>
        ///     Retrieves the nursing notes associated with the specified admission identifier.
        /// </summary>
        /// <remarks>This action is accessible only to users in the Doctor or Nurse roles.</remarks>
        /// <param name="admissionId">The unique identifier of the admission for which to retrieve nursing notes. Cannot be null or empty.</param>
        /// <returns>An IActionResult containing the nursing notes if found; otherwise, a NotFound result.</returns>
        [HttpGet]
        [Authorize(Roles ="Doctor,Nurse")]
        public async Task<IActionResult> Get(string admissionId)
        {
            var result = await Sender.Send(
                new GetNursingNotesQuery(admissionId));

            return result is not null ? Ok(result) : NotFound();
        }
        /// <summary>
        ///     Updates an existing nursing note for the specified admission.
        /// </summary>
        /// <remarks>This action requires the caller to have the 'Nurse' role. The admissionId in the
        /// route must match the Id in the command payload.</remarks>
        /// <param name="admissionId">The unique identifier of the admission associated with the nursing note to update. Cannot be null or empty.</param>
        /// <param name="command">The command containing the updated nursing note data. The command's Id must match the specified admissionId.</param>
        /// <returns>An IActionResult indicating the result of the update operation. Returns NoContent if the update is
        /// successful, BadRequest if the admissionId does not match the command Id, or NotFound if the nursing note
        /// does not exist.</returns>
        [HttpPut]
        [Authorize(Roles = "Nurse")]

        public async Task<IActionResult> Update(string admissionId, [FromBody] UpdateNursingNoteCommand command)
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
        /// Deletes a nursing note associated with the specified admission and note identifiers.
        /// </summary>
        /// <remarks>Requires the caller to be authorized with the Nurse role.</remarks>
        /// <param name="admissionId">The unique identifier of the admission to which the nursing note belongs. Cannot be null or empty.</param>
        /// <param name="Id">The unique identifier of the nursing note to delete. Cannot be null or empty.</param>
        /// <returns>A 204 No Content response if the nursing note was successfully deleted; otherwise, a 404 Not Found response
        /// if the note does not exist.</returns>
        [HttpDelete]
        [Authorize(Roles = "Nurse")]

        public async Task<IActionResult> Delete(string admissionId, string Id)
        {
            var success = await Sender.Send(new DeleteNursingNoteCommand(admissionId, Id));

            if (!success) return NotFound();

            return NoContent(); // 204 No Content
        }
    }
}