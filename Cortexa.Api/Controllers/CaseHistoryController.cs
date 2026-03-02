using Cortexa.Application.Features.ClinicalData.Commands;
using Cortexa.Application.Features.ClinicalData.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    [ApiController]
    [Route("api/admissions/{admissionId}/case-history")]
    public class CaseHistoryController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Add(
            string admissionId,
            [FromBody] AddCaseHistoryCommand command)
        {
            command.AdmissionId = admissionId;

            var id = await Sender.Send(command);

            return Created($"{Request.Path}/{id}", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string admissionId)
        {
            var result = await Sender.Send(
                new GetCaseHistoryQuery(admissionId));

            return Ok(result);
        }
    }
}
