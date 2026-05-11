using Cortexa.Application.Features.Rooms.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/Rooms")]
    public class RoomsController(ISender sender) : ApiControllerBase(sender)
    {
        /// <summary>
        /// Returns all rooms in the system. Only accessible by users with the roles "Doctor" or "Nurse".
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of rooms if found; otherwise, a <see cref="NotFoundResult"/>.</returns>

        [HttpGet]
        [Authorize(Roles ="Doctor,Nurse")]
        public async Task<IActionResult> GetAllRooms()
        {
            var result = await Sender.Send(new GetRoomsQuery());
            return result is not null ? Ok(result) : NotFound();
        }

    }
    }