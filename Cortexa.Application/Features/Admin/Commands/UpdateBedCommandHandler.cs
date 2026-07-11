using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Infrastructure;
using Cortexa.Domain.Enums;
using MediatR;

namespace Cortexa.Application.Features.Admin.Commands
{
    public record UpdateBedCommand(string BedId,string RoomId, string BedNumber, BedStatus Status) : IRequest<ResultDto<bool>>;

    public class UpdateBedCommandHandler : IRequestHandler<UpdateBedCommand, ResultDto<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateBedCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultDto<bool>> Handle(UpdateBedCommand request, CancellationToken cancellationToken)
        {
            var bed = await _unitOfWork.Beds
                .GetByIdAsync(request.BedId);
            if (bed == null) return new ResultDto<bool> { Data = false, Success = false, Message = "Bed not found" };
            var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId);
            if (room == null) return new ResultDto<bool> { Data = false, Success = false, Message = "Room not found" };
            bed.BedNumber = request.BedNumber;
            bed.Status = request.Status;
            bed.RoomId = request.RoomId;
            await _unitOfWork.Beds.UpdateAsync(bed);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new ResultDto<bool> { Data = true, Success = true, Message = "Bed updated successfully" };
        }
    }
}