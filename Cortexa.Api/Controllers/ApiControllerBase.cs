using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    /// <summary>
    /// Base controller providing shared MediatR sender for all API controllers.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender? _sender;

        /// <summary>
        /// Lazily resolves ISender from the DI container.
        /// </summary>
        protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
