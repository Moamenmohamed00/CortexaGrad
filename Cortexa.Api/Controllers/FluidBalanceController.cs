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
    [Route("api/admissions/{admissionId}/fluid-balance")]
    [Authorize]
    public class FluidBalanceController(ISender sender) : ApiControllerBase(sender)
    {
        /// <summary>
        ///   Adds a new fluid balance record for a specific admission.
        /// </summary>
        /// <param name="admissionId">The unique identifier of the admission.</param>
        /// <param name="command">The details of the fluid balance record to add.</param>
        /// <returns>The ID of the newly created fluid balance record.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Add(
            string admissionId,
            [FromBody] AddFluidBalanceCommand command)
        {
            command.AdmissionId = admissionId;

            var id = await Sender.Send(command);

            return Created($"{Request.Path}/{id}", new { id });
        }
        /// <summary>
        /// Retrieves the fluid balance information for the specified admission.    
        /// </summary>
        /// <remarks>This action requires the caller to be authorized with the Doctor or Nurse
        /// role.</remarks>
        /// <param name="admissionId">The unique identifier of the admission for which to retrieve fluid balance data. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the fluid balance information if found; otherwise, a NotFound
        /// result.</returns>
        [HttpGet]
        [Authorize(Roles ="Doctor,Nurse")]
        public async Task<IActionResult> Get(string admissionId)
        {
            var result = await Sender.Send(
                new GetFluidBalanceQuery(admissionId));

            return result is not null ? Ok(result) : NotFound();
        }


        /// <summary>
        /// Updates the fluid balance record for the specified admission.
        /// </summary>
        /// <param name="admissionId">The unique identifier of the admission whose fluid balance is to be updated. Cannot be null or empty.</param>
        /// <param name="command">The command containing the updated fluid balance data. The command's Id property must match the admissionId
        /// parameter.</param>
        /// <returns>An IActionResult indicating the result of the update operation. Returns 204 No Content if the update is
        /// successful, 400 Bad Request if the admissionId does not match the command's Id, or 404 Not Found if the
        /// record does not exist.</returns>
        [HttpPut]
        public async Task<IActionResult> Update(string admissionId, [FromBody] UpdateFluidBalanceCommand command)
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
        /// Deletes the specified fluid balance record for the given admission.
        /// </summary>
        /// <param name="admissionId">The unique identifier of the admission associated with the fluid balance record to delete. Cannot be null or
        /// empty.</param>
        /// <param name="Id">The unique identifier of the fluid balance record to delete. Cannot be null or empty.</param>
        /// <returns>A 204 No Content response if the deletion is successful; otherwise, a 404 Not Found response if the record
        /// does not exist.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(string admissionId, string Id)
        {
            var success = await Sender.Send(new DeleteFluidBalanceCommand(admissionId, Id));

            if (!success) return NotFound();

            return NoContent(); // 204 No Content
        }
    }
}
