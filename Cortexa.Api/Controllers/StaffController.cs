using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cortexa.Domain.Constants;
namespace Cortexa.Api.Controllers
{
    [ApiController]
    [Route("api/Staff")]
    [Authorize(Roles = $"{AppRoles.Doctor},{AppRoles.Nurse},{AppRoles.Admin}")]

    /// <summary>
    /// Endpoints for staff management.
    /// Currently a placeholder — endpoints will be enabled once the
    /// underlying MediatR queries are implemented.
    /// </summary>
    public class StaffController(ISender sender) : ApiControllerBase(sender)
    {
    }
}
