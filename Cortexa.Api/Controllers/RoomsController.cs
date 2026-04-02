using Cortexa.Application.Features.Rooms.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cortexa.Api.Controllers
{
    public class RoomsController(ISender sender) : ApiControllerBase(sender)
    {

        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            var result = await Sender.Send(new GetRoomsQuery());
            return result is not null ? Ok(result) : NotFound();
        }

    }
    }