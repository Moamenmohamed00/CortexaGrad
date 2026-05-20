using Cortexa.Application.Dtos.Admin;
using Cortexa.Application.Dtos.AI;
using Cortexa.Application.Dtos.AuditLog;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Features.Admin.Commands;
using Cortexa.Application.Features.Admin.Queries;
using Cortexa.Application.Features.Rooms.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{

    // Cortexa.Api/Controllers/AdminDashboardController.cs
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin-dashboard")]
    public class AdminDashboardController(ISender sender) : ApiControllerBase(sender)
    {

        /// <summary>
        /// Retrieves a summary of dashboard metrics and statistics for the current user.
        /// </summary>
        /// <remarks>Use this endpoint to obtain an overview of key dashboard data, such as counts or
        /// aggregated metrics relevant to the user's context. The exact contents of the summary are defined by <see
        /// cref="DashboardSummaryDto"/>.</remarks>
        /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="DashboardSummaryDto"/> with the dashboard summary
        /// data. Returns a 200 OK response with the summary information.</returns>
        [HttpGet("summary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
        {
            return Ok(await Sender.Send(new GetDashboardSummaryQuery()));
        }

        /// <summary>
        /// Retrieves a paged list of system audit logs based on the specified query parameters.
        /// </summary>
        /// <param name="query">The query parameters used to filter, sort, and paginate the audit logs.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ActionResult{T}"/>
        /// with a <see cref="PagedResult{AuditLogResponseDto}"/> representing the paged audit logs.</returns>
        [HttpGet("audit-logs")]
        public async Task<ActionResult<PagedResult<AuditLogResponseDto>>> GetAuditLogs(
            [FromQuery] GetSystemAuditLogsQuery query)
        {
            var result = await Sender.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Creates a new administrator account using the specified command.
        /// </summary>
        /// <param name="command">The command containing the details required to create the administrator account. Cannot be null.</param>
        /// <returns>An IActionResult indicating the result of the operation. Returns 200 OK with the result if successful;
        /// otherwise, returns 400 Bad Request with error details.</returns>
        [HttpPost("create-admin")]

        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminCommand command)
        {
            var result = await Sender.Send(command);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Handles HTTP GET requests to retrieve the list of available roles.
        /// </summary>
        /// <remarks>Use this endpoint to obtain all roles defined in the system. The response format and
        /// content depend on the implementation of the underlying query handler.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing the collection of roles. Returns an HTTP 200 response with the
        /// roles if successful.</returns>
        [HttpGet("roles")]

        public async Task<IActionResult> GetRoles()
        {
            var roles = await Sender.Send(new GetRolesQuery());
            return Ok(roles);
        }
        /// <summary>
        /// Creates a new role based on the specified command.
        /// </summary>
        /// <remarks>This endpoint is typically used by administrators to add new roles to the system. The
        /// response includes information about the success or failure of the operation.</remarks>
        /// <param name="command">The command containing the details required to create the role. Cannot be null.</param>
        /// <returns>An IActionResult indicating the result of the operation. Returns 200 OK with the result if successful;
        /// otherwise, returns 400 Bad Request with error details.</returns>

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
        {
            var result = await Sender.Send(command);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Deletes the role with the specified identifier.
        /// </summary>
        /// <param name="roleId">The unique identifier of the role to delete. Cannot be null or empty.</param>
        /// <returns>An IActionResult indicating the result of the delete operation. Returns 200 OK if the role was deleted
        /// successfully; otherwise, returns 400 Bad Request with error details.</returns>

        [HttpDelete("delete-role/{roleId}")]
        public async Task<IActionResult> DeleteRole([FromRoute] string roleId)
        {
            var result = await Sender.Send(new DeleteRoleCommand(roleId));
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Assigns a role to a user based on the specified command.
        /// </summary>
        /// <param name="command">An object containing the details required to assign a role to a user. Cannot be null.</param>
        /// <returns>An IActionResult indicating the result of the operation. Returns 200 OK if the role was assigned
        /// successfully; otherwise, returns 400 Bad Request with error details.</returns>
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleToUserCommand command)
        {
            var result = await Sender.Send(command);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Removes a specified role from a user based on the provided command.
        /// </summary>
        /// <param name="command">The command containing the user and role information required to remove the role. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IActionResult indicating the
        /// outcome of the operation: 200 OK if the role was removed successfully; otherwise, 400 Bad Request with error
        /// details.</returns>
        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRole([FromBody] RemoveRoleFromUserCommand command)
        {
            var result = await Sender.Send(command);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of users along with their associated roles.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the result of the operation. Returns an HTTP 200 response with the
        /// list of users and their roles if successful; otherwise, returns an HTTP 400 response with error details.</returns>
        [HttpGet("users-with-roles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var result = await Sender.Send(new GetUsersWithRolesQuery());
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Toggles the status of a user based on the provided command.
        /// </summary>
        /// <param name="command">The command containing the user identifier and the desired status change. Cannot be null.</param>
        /// <returns>An IActionResult indicating the outcome of the operation. Returns 200 OK with the result if successful;
        /// otherwise, returns 400 Bad Request with error details.</returns>
        [HttpPost("toggle-user-status")]
        public async Task<IActionResult> ToggleUserStatus([FromBody] ToggleUserStatusCommand command)
        {
            var result = await Sender.Send(command);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Forces a password reset for a user based on the specified command.
        /// </summary>
        /// <remarks>This endpoint is typically used by administrators to require a user to reset their
        /// password. The operation may fail if the command is invalid or if the user cannot be found.</remarks>
        /// <param name="command">The command containing the details required to perform the password reset. Cannot be null.</param>
        /// <returns>An IActionResult indicating the outcome of the password reset operation. Returns a success response if the
        /// reset is completed; otherwise, returns a bad request with error details.</returns>
        [HttpPost("force-reset-password")]
        public async Task<IActionResult> ForceResetPassword([FromBody] ForceResetPasswordCommand command)
        {
            var result = await Sender.Send(command);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new room based on the specified command and returns the result of the operation.
        /// </summary>
        /// <param name="command">The command containing the details required to create the room. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IActionResult indicating the
        /// outcome of the room creation request.</returns>
        [HttpPost("create-room")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomCommand command)
        {
            var result = await Sender.Send(command);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Updates the details of an existing room based on the specified command.
        /// </summary>
        /// <param name="command">An object containing the updated room information and the criteria for identifying the room to update.
        /// Cannot be null.</param>
        /// <returns>An IActionResult indicating the result of the update operation. Returns 200 OK with the result if the update
        /// is successful; otherwise, returns 400 Bad Request with error details.</returns>
        [HttpPost("update-room")]
        public async Task<IActionResult> UpdateRoom([FromBody] UpdateRoomCommand command)
        {
            var result = await Sender.Send(command);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new bed resource using the specified command.
        /// </summary>
        /// <param name="command">The command containing the details required to create the bed. Cannot be null.</param>
        /// <returns>An IActionResult that represents the result of the create operation. Returns 200 OK with the result if
        /// successful; otherwise, returns 400 Bad Request with error details.</returns>

        [HttpPost("create-bed")]
        public async Task<IActionResult> CreateBed([FromBody] CreateBedCommand command)
        {
            var result = await Sender.Send(command);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Updates the details of a bed using the specified update command.
        /// </summary>
        /// <remarks>This endpoint is typically used to modify bed information in the system. The request
        /// body must contain a valid UpdateBedCommand object.</remarks>
        /// <param name="command">An object containing the information required to update the bed. Must not be null.</param>
        /// <returns>An IActionResult indicating the result of the update operation. Returns 200 OK with the result if
        /// successful; otherwise, returns 400 Bad Request with error details.</returns>
        [HttpPost("update-bed")]
        public async Task<IActionResult> UpdateBed([FromBody] UpdateBedCommand command)
        {
            var result = await Sender.Send(command);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Deletes the bed with the specified identifier.
        /// </summary>
        /// <param name="bedId">The unique identifier of the bed to delete. Cannot be null or empty.</param>
        /// <returns>An IActionResult indicating the result of the delete operation. Returns 200 OK if the bed was deleted
        /// successfully; otherwise, returns 400 Bad Request with error details.</returns>
        [HttpDelete("delete-bed/{bedId}")]
        public async Task<IActionResult> DeleteBed([FromRoute] string bedId)
        {
            var result = await Sender.Send(new DeleteBedCommand(bedId));
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Toggles the availability status of a room based on the specified command.
        /// </summary>
        /// <param name="command">An object containing the details required to identify the room and the desired availability state. Cannot be
        /// null.</param>
        /// <returns>An IActionResult indicating the outcome of the operation. Returns 200 OK with the result if successful;
        /// otherwise, returns 400 Bad Request with error details.</returns>

        [HttpPut("toggle-room-availability")]
        public async Task<IActionResult> ToggleRoomAvailability([FromBody] ToggleRoomAvailabilityCommand command)
        {
            var result = await Sender.Send(command);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        //[HttpGet("ai-insights")]
        //public async Task<ActionResult<List<RagQueryDto>>> GetAIInsights()
        //{
        //    return Ok(await Mediator.Send(new GetRAGPerformanceQuery()));
        //}
    }
}
