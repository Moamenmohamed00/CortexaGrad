using Cortexa.Application.Features.ClinicalData.Commands;
using Cortexa.Application.Features.ClinicalData.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    [ApiController]
    [Route("api/admissions/{admissionId}/intervention-procedures")]
    public class InterventionProceduresController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Add(
            string admissionId,
            [FromBody] AddInterventionProcedureCommand command)
        {
            command.AdmissionId = admissionId;

            var id = await Sender.Send(command);

            return Created($"{Request.Path}/{id}", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string admissionId)
        {
            var result = await Sender.Send(
                new GetInterventionProcedureQuery(admissionId));

            return Ok(result);
        }
    }
}
