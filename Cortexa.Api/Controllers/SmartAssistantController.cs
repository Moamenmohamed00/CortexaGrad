using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    // [ApiController]
    [Route("api/SmartAssistant")]
    /// <summary>
    /// Endpoints for the AI Smart Assistant features (alerts, RAG queries).
    /// Currently a placeholder — endpoints will be enabled once the
    /// underlying MediatR commands/queries are implemented.
    /// </summary>
    public class SmartAssistantController(ISender sender) : ApiControllerBase(sender)
    {
    }
}
