using MediatR;
using Microsoft.AspNetCore.Mvc;
using Cortexa.Application.Features.Nurses;
namespace Cortexa.Api.Controllers
{
    [ApiController]
    [Route("api/Nurses")]
    public class NursesController(ISender sender) : ApiControllerBase(sender)
    {
        /// <summary>
        /// Gets all Nurses.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of all nurses if found; otherwise, a <see cref="NotFoundResult"/>.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await Sender.Send(new GetAllNursesQuery());
            return result is not null ? Ok(result) : NotFound();
        }


        /// <summary>
        /// Retrieves detailed information for the nurse associated with the specified email address.
        /// </summary>
        /// <param name="email">The email address of the nurse whose details are to be retrieved. Cannot be null or empty.</param>
        /// <returns>An <see cref="OkObjectResult"/> containing the nurse details if found; otherwise, a <see
        /// cref="NotFoundResult"/> if no nurse exists with the specified email.</returns>
        [HttpGet("{email}/details")]
        public async Task<IActionResult> GetDetails(string email)
        {
            var result = await Sender.Send(new GetNurseByEmailQuery(email));
            return result is not null
                ? Ok(result)
                : NotFound();

        }

        /// <summary>
        /// Retrieves detailed information about all nurses assigned to the specified department.
        /// </summary>
        /// <param name="department">The name of the department for which to retrieve nurse details. Cannot be null or empty.</param>
        /// <returns>An IActionResult containing the details of nurses in the specified department if found; otherwise, a
        /// NotFound result.</returns>
        [HttpGet("{department}/department/details")]
        public async Task<IActionResult> GetByDepartment(string department)
        {
            var result = await Sender.Send(new GetAllNursesByDepartmentQuery(department));
            return result is not null
                ? Ok(result)
                : NotFound();
        }
    }
}
