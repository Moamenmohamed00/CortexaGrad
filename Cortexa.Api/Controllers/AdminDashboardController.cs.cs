using Cortexa.Application.Dtos.Admin;
using Cortexa.Application.Dtos.AI;
using Cortexa.Application.Dtos.AuditLog;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Features.Admin.Queries;
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

        //[HttpGet("ai-insights")]
        //public async Task<ActionResult<List<RagQueryDto>>> GetAIInsights()
        //{
        //    return Ok(await Mediator.Send(new GetRAGPerformanceQuery()));
        //}
    }
}
