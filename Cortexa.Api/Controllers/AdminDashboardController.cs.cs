using CloudinaryDotNet.Actions;
using Cortexa.Api.Extensions;
using Cortexa.Application.Dtos.Admin;
using Cortexa.Application.Dtos.AI;
using Cortexa.Application.Dtos.AuditLog;
using Cortexa.Application.Dtos.Auth;
using Cortexa.Application.Dtos.Beds;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Dtos.Rooms;
using Cortexa.Application.Dtos.Schedule;
using Cortexa.Application.Features.Admin.Commands;
using Cortexa.Application.Features.Admin.Queries;
using Cortexa.Application.Features.Auth;
using Cortexa.Application.Features.Rooms.Queries;
using Cortexa.Application.Features.Schedule.Commands;
using Cortexa.Application.Features.Schedule.Queries;
using Cortexa.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;
namespace Cortexa.Api.Controllers
{

    // Cortexa.Api/Controllers/AdminDashboardController.cs
    //[Authorize(Roles = $"{AppRoles.Admin}")]
    [ApiController]
    [Route("api/admin-dashboard")]
    public class AdminDashboardController(ISender sender) : ApiControllerBase(sender)
    {

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
        /// Adds a new user to the system using the specified user details.
        /// rules for adding user: Doctor or Nurse Only
        /// </summary>
        /// <remarks>This action requires the caller to have the Admin role.</remarks>
        /// <param name="command">An object containing the information required to create the new user. Cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the operation. Returns status code 200 (OK) if
        /// the user is added successfully; otherwise, returns status code 400 (Bad Request) if the input is invalid.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("users")]

        public async Task<IActionResult> AddUser([FromBody] AddUserRequestDto command)
        {
            var result = await Sender.Send(new AddUserCommand(command));
            return Ok(result);
        }

        /// <summary>
        /// Updates the details of an existing doctor user.
        /// </summary>
        /// <param name="email">The email of the doctor to update.</param>
        /// <param name="request">An object containing the updated information for the doctor user.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the operation.</returns>
        [HttpPut("doctor-user/{email}")]
        public async Task<IActionResult> UpdateDoctorUser([FromRoute] string email, [FromBody] UpdateDoctorUserRequestDto request)
        {
            var result = await Sender.Send(new UpdateDoctorUserCommand(email, request));
            return Ok(result);
        }

        /// <summary>
        /// Updates the details of an existing nurse user.
        /// </summary>
        /// <param name="email">The email of the nurse to update.</param>
        /// <param name="request">An object containing the updated information for the nurse user.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the operation.</returns>
        [HttpPut("nurse-user/{email}")]
        public async Task<IActionResult> UpdateNurseUser([FromRoute] string email, [FromBody] UpdateNurseUserRequestDto request)
        {
            var result = await Sender.Send(new UpdateNurseUserCommand(email, request));
            return Ok(result);
        }

        /// <summary>
        /// Toggles the status of a user.
        /// </summary>
        /// <param name="id">The ID of the user whose status is to be toggled.</param>
        /// <returns>An IActionResult indicating the outcome of the operation.</returns>
        [HttpPost("users/{id}/toggle-status")]
        public async Task<IActionResult> ToggleUserStatus([FromRoute] string id)
        {
            var result = await Sender.Send(new ToggleUserStatusCommand(id));
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Forces a password reset for a specific user.
        /// </summary>
        /// <param name="id">The ID of the user whose password needs to be reset.</param>
        /// <param name="newpassword">The new password to set for the user.</param>
        /// <returns>An IActionResult indicating the outcome of the operation.</returns>
        [HttpPost("users/{id}/force-password-reset")]
        public async Task<IActionResult> ForceResetPassword([FromRoute] string id, [FromBody] string newpassword)
        {
            // بناء الـ Command داخل الـ Controller لحماية المعمارية وفصل الطبقات
            var result = await Sender.Send(new ForceResetPasswordCommand(id, newpassword));

            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }


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
        [HttpPost("roles")]
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

        [HttpDelete("roles/{roleId}")]
        public async Task<IActionResult> DeleteRole([FromRoute] string roleId)
        {
            var result = await Sender.Send(new DeleteRoleCommand(roleId));
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="roleName">The name of the role to assign.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("users/{userId}/roles/{roleName}")]
        public async Task<IActionResult> AssignRole([FromRoute] string userId, [FromRoute] string roleName)
        {
            var result = await Sender.Send(new AssignRoleToUserCommand(userId, roleName));
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Removes a specified role from a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="roleName">The name of the role to remove.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpDelete("users/{userId}/roles/{roleName}")]
        public async Task<IActionResult> RemoveRole([FromRoute] string userId, [FromRoute] string roleName)
        {
            var result = await Sender.Send(new RemoveRoleFromUserCommand(userId, roleName));
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
        /// <param name="roomId">The ID of the room to update.</param>
        /// <param name="request">An object containing the updated room information and the criteria for identifying the room to update.    
        /// Cannot be null.</param>
        /// <returns>An IActionResult indicating the result of the update operation. Returns 200 OK with the result if the update
        /// is successful; otherwise, returns 400 Bad Request with error details.</returns>
        [HttpPut("rooms/{roomId}")]
        public async Task<IActionResult> UpdateRoom([FromRoute] string roomId, [FromBody] UpdateRoomDto request)
        {
            var result = await Sender.Send(new UpdateRoomCommand(
                roomId,
                request.RoomNumber,
                request.RoomType,
                request.Capacity,
                request.IsAvailable
            ));

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new bed resource using the specified command.
        /// </summary>
        /// <param name="command">The command containing the details required to create the bed. Cannot be null.</param>
        /// <returns>An IActionResult that represents the result of the create operation. Returns 200 OK with the result if
        /// successful; otherwise, returns 400 Bad Request with error details.</returns>

        [HttpPost("beds")]
        public async Task<IActionResult> CreateBed([FromBody] CreateBedCommand command)
        {
            var result = await Sender.Send(command);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Updates the details of a bed.
        /// </summary>
        /// <param name="bedId">The unique identifier of the bed to update.</param>
        /// <param name="request">An object containing the information required to update the bed.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut("beds/{bedId}")]
        public async Task<IActionResult> UpdateBed([FromRoute] string bedId, [FromBody] UpdateBedDto request)
        {
            var result = await Sender.Send(new UpdateBedCommand(
                bedId,
                request.RoomId,
                request.BedNumber,
                request.Status
            ));

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Deletes the bed with the specified identifier.
        /// </summary>
        /// <param name="bedId">The unique identifier of the bed to delete. Cannot be null or empty.</param>
        /// <returns>An IActionResult indicating the result of the delete operation. Returns 200 OK if the bed was deleted
        /// successfully; otherwise, returns 400 Bad Request with error details.</returns>
        [HttpDelete("bed/{bedId}")]
        public async Task<IActionResult> DeleteBed([FromRoute] string bedId)
        {
            var result = await Sender.Send(new DeleteBedCommand(bedId));
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Toggles the availability status of a room based on the specified command.
        /// </summary>
        /// <param name="roomId">The unique identifier of the room to toggle availability. Cannot be null or empty.</param>
        /// <returns>An IActionResult indicating the outcome of the operation. Returns 200 OK with the result if successful;
        /// otherwise, returns 400 Bad Request with error details.</returns>

        [HttpPost("room/{roomId}/toggle-availability")]
        public async Task<IActionResult> ToggleRoomAvailability([FromRoute] string roomId)
        {
            var result = await Sender.Send(new ToggleRoomAvailabilityCommand(roomId));
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new recurring schedule for the specified staff member.
        /// </summary>
        /// <param name="staffId">The unique identifier of the staff member for whom the schedule is being created. Cannot be null or empty.</param>
        /// <param name="request">The details of the recurring schedule to create. Must not be null.</param>
        /// <returns>An IActionResult indicating the result of the operation. Returns 200 OK with the result if successful;
        /// otherwise, returns 400 Bad Request with error details.</returns>
        [HttpPost("staff/{staffId}/schedules")]
        public async Task<IActionResult> CreateStaffSchedule([FromRoute] string staffId, [FromBody] CreateStaffScheduleDto request) 
        {
            var result = await Sender.Send(new CreateStaffScheduleCommand(staffId, request));
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }


        /// <summary>
        /// Retrieves the schedules associated with a specific staff member.
        /// </summary>
        /// <param name="staffId">The unique identifier of the staff member whose schedules are being retrieved. Cannot be null or empty.</param>
        /// <returns>An IActionResult containing the schedules of the specified staff member. Returns 200 OK with the schedules if successful;
        /// otherwise, returns 400 Bad Request with error details.</returns>
        [HttpGet("staff/{staffId}/schedules")]
        public async Task<IActionResult> GetStaffSchedules([FromRoute] string staffId)
        {
            var result = await Sender.Send(new SchedulesByStaffIdCommand(staffId));
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
