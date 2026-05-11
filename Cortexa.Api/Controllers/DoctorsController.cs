using Cortexa.Application.Features.Doctors.Queries;
using Cortexa.Application.Features.Patients.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    public class DoctorsController(ISender sender) : ApiControllerBase(sender)
    {

        /// <summary>
        /// Gets detailed Doctor by email.
        /// </summary>
        /// <param name="email">The email of the doctor to retrieve details for. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the doctor's details if found; otherwise, a <see cref="NotFoundResult"/>.</returns>
        [HttpGet("{email}/details")]
        public async Task<IActionResult> GetDetails(string email)
        {
            var result = await Sender.Send(new GetDoctorByEmailQuery(email));

            return result is not null
                ? Ok(result)
                : NotFound();
        }
        /// <summary>
        /// Gets detailed Doctor by specialization.
        /// </summary>
        /// <param name="specialization">The specialization of the doctors to retrieve details for. Cannot be null or empty.</param>
        /// <returns>An <see cref="IActionResult"/> containing the doctors' details if found; otherwise, a <see cref="NotFoundResult"/>.</returns>
        [HttpGet("{specialization}/specialization/details")]
        public async Task<IActionResult> GetDetailsBySpecialization(string specialization)
        {
            var result = await Sender.Send(new GetAllDoctorsBySpecializationQuery(specialization));

            return result is not null
                ? Ok(result)
                : NotFound();
        }

        /// <summary>
        /// Gets all Doctors.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of all doctors if found; otherwise, a <see cref="NotFoundResult"/>.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await Sender.Send(new GetAllDoctorsQuery());
            return result is not null ? Ok(result) : NotFound();
        }

    }

}