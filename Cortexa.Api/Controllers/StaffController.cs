using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    // [ApiController]
    [Route("api/Staff")]
    /// <summary>
    /// Endpoints for staff management.
    /// Currently a placeholder — endpoints will be enabled once the
    /// underlying MediatR queries are implemented.
    /// </summary>
    public class StaffController(ISender sender) : ApiControllerBase(sender)
    {
    }
}
