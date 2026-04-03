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

        [HttpGet]
        [Authorize(Roles ="Doctor,Nurse")]
        public async Task<IActionResult> GetAllRooms()
        {
            var result = await Sender.Send(new GetRoomsQuery());
            return result is not null ? Ok(result) : NotFound();
        }

    }
    }