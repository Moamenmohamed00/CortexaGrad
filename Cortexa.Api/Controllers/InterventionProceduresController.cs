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
    [Route("api/admissions/{admissionId}/intervention-procedures")]
    [Authorize]
    public class InterventionProceduresController(ISender sender) : ApiControllerBase(sender)
    {
        /// <summary>
        /// Adds a new intervention procedure to an admission.
        /// </summary>
        /// <param name="admissionId">The unique identifier of the admission.</param>
        /// <param name="command">The details of the intervention procedure to add.</param>
        /// <returns>The ID of the newly created intervention procedure.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(
            string admissionId,
            [FromBody] AddInterventionProcedureCommand command)
        {
            command.AdmissionId = admissionId;

            var id = await Sender.Send(command);

            return Created($"{Request.Path}/{id}", new { id });
        }
        /// <summary>
        /// Retrieves the intervention procedure details for the specified admission identifier.
        /// </summary>
        /// <remarks>This action requires the caller to be authorized with the Doctor or Nurse
        /// role.</remarks>
        /// <param name="admissionId">The unique identifier of the admission for which to retrieve intervention procedure details. Cannot be null.</param>
        /// <returns>An IActionResult containing the intervention procedure details if found; otherwise, a NotFound result.</returns>
        [HttpGet]
        [Authorize(Roles ="Doctor,Nurse")]
        public async Task<IActionResult> Get(string admissionId)
        {
            var result = await Sender.Send(
                new GetInterventionProcedureQuery(admissionId));

            return result is not null ? Ok(result) : NotFound();
        }

        /// <summary>
        /// Updates an existing intervention procedure for the specified admission.
        /// </summary>
        /// <param name="admissionId">The unique identifier of the admission associated with the intervention procedure to update.</param>
        /// <param name="command">The command containing the updated intervention procedure data. Must include a valid identifier matching the
        /// admission.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see
        /// cref="BadRequestResult"/> if the admission ID does not match the command ID, <see cref="NotFoundResult"/> if
        /// the update fails, or <see cref="NoContentResult"/> if the update is successful.</returns>

        [HttpPut]
        public async Task<IActionResult> Update(string admissionId, [FromBody] UpdateInterventionProcedureCommand command)
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
        /// Deletes the specified intervention procedure associated with the given admission.
        /// </summary>
        /// <param name="admissionId">The unique identifier of the admission to which the intervention procedure belongs. Cannot be null or empty.</param>
        /// <param name="Id">The unique identifier of the intervention procedure to delete. Cannot be null or empty.</param>
        /// <returns>A 204 No Content response if the deletion is successful; otherwise, a 404 Not Found response if the
        /// specified intervention procedure does not exist.</returns>

        [HttpDelete]
        public async Task<IActionResult> Delete(string admissionId, string Id)
        {
            var success = await Sender.Send(new DeleteInterventionProcedureCommand(admissionId, Id));

            if (!success) return NotFound();

            return NoContent(); // 204 No Content
        }
    }
}
