using Cortexa.Application.Features.Doctors.Queries;
using Cortexa.Application.Features.Patients.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    public class DoctorsController(ISender sender) : ApiControllerBase(sender)
    {

        /// <summary>
        /// Gets detailed Doctor.
        /// </summary>
        [HttpGet("{email}/details")]
        public async Task<IActionResult> GetDetails(string email)
        {
            var result = await Sender.Send(new GetDoctorByEmailQuery(email));

            return result is not null
                ? Ok(result)
                : NotFound();
        }
        /// <summary>
        /// Gets detailed Doctor.
        /// </summary>
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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await Sender.Send(new GetAllDoctorsQuery());
            return Ok(result);
        }

    }

}