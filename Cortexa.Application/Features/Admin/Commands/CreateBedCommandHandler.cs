using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Enums;
using MediatR;

namespace Cortexa.Application.Features.Admin.Commands
{
    public record CreateBedCommand(string RoomId, string BedNumber) : IRequest<ResultDto<string>>;

    public class CreateBedCommandHandler : IRequestHandler<CreateBedCommand, ResultDto<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBedCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultDto<string>> Handle(CreateBedCommand request, CancellationToken cancellationToken)
        {
            var room = await _unitOfWork.Rooms
                .GetByIdAsync(request.RoomId);

            if (room == null) return new ResultDto<string> { Data = null, Success = false, Message = "Room not found" };

            if (room.Beds.Count >= room.Capacity)
            {
                return new ResultDto<string> { Data = null, Success = false, Message = "Cannot add more beds. Room capacity reached." };
            }

            var newBed = new Domain.Entities.Infrastructure.Bed
            {
                RoomId = request.RoomId,
                BedNumber = request.BedNumber,
                Status = BedStatus.Available
            };

            await _unitOfWork.Beds.AddAsync(newBed);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ResultDto<string> { Data = newBed.Id, Success = true, Message = "Bed created successfully" };
        }
    }
}