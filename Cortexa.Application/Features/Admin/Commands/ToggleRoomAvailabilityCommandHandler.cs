using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.Admin.Commands
{
    public record ToggleRoomAvailabilityCommand(string RoomId) : IRequest<ResultDto<bool>>;

    public class ToggleRoomAvailabilityCommandHandler : IRequestHandler<ToggleRoomAvailabilityCommand, ResultDto<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ToggleRoomAvailabilityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultDto<bool>> Handle(ToggleRoomAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var room = await _unitOfWork.Rooms
                .GetByIdAsync(request.RoomId);
            if (room == null) return new ResultDto<bool> { Data = false, Success = false, Message = "Room not found" };
            room.IsAvailable = !room.IsAvailable;
            await _unitOfWork.Rooms.UpdateAsync(room);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new ResultDto<bool> { Data = true, Success = true, Message = $"Room availability toggled to {(room.IsAvailable ? "Available" : "Unavailable")}" };
        }
    }
}