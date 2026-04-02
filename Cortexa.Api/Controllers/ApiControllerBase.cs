using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    /// <summary>
    /// Base controller providing shared MediatR sender for all API controllers.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase(ISender sender) : ControllerBase
    {
        protected ISender Sender => sender;
    }
}
