using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Infrastructure;
using Cortexa.Domain.Enums;
using MediatR;

namespace Cortexa.Application.Features.Admin.Commands
{
    public record CreateRoomCommand(string RoomNumber, RoomType RoomType, int Capacity) : IRequest<ResultDto<string>>;

    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, ResultDto<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultDto<string>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            // Check if room number already exists
            var existingRoom = await _unitOfWork.Rooms
                .FindAsync(r => r.RoomNumber == request.RoomNumber);
            if (existingRoom != null)
            {
                return new ResultDto<string> { Data = null, Success = false, Message = "A room with this number already exists." };
            }
            var newRoom = new Room
            {
                RoomNumber = request.RoomNumber,
                Type = request.RoomType,
                Capacity = request.Capacity,
                IsAvailable = true
            };
            await _unitOfWork.Rooms.AddAsync(newRoom);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new ResultDto<string> { Data = newRoom.Id, Success = true, Message = "Room created successfully" };
        }
    }
}