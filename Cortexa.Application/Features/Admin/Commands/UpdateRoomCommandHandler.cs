using Cortexa.Application.Common.Interfaces;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.Admin.Commands
{
    public record UpdateRoomCommand(string Id, string RoomNumber, RoomType RoomType, int Capacity, bool IsAvailable) : IRequest<ResultDto<bool>>;
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, ResultDto<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultDto<bool>> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await _unitOfWork.Rooms
                .GetByIdAsync(request.Id);

            if (room == null) return new ResultDto<bool> { Data = false, Success = false, Message = "Room not found" };

            // Ensure the new room number is not taken by another room
            var roomNumberTaken = await _unitOfWork.Rooms
                .FindAsync(r => r.RoomNumber == request.RoomNumber && r.Id != request.Id);

            if (roomNumberTaken!=null)
            {
                return new ResultDto<bool> { Data = false, Success = false, Message = "The new room number is already assigned to another room." };
            }

            room.RoomNumber = request.RoomNumber;
            room.Type = request.RoomType;
            room.Capacity = request.Capacity;
            room.IsAvailable = request.IsAvailable;

            await _unitOfWork.Rooms.UpdateAsync(room);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ResultDto<bool> { Data = true, Success = true, Message = "Room updated successfully" };
        }
    }

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

    public record DeleteBedCommand(string BedId) : IRequest<ResultDto<bool>>;

    public class DeleteBedCommandHandler : IRequestHandler<DeleteBedCommand, ResultDto<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteBedCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultDto<bool>> Handle(DeleteBedCommand request, CancellationToken cancellationToken)
        {
            var bed = await _unitOfWork.Beds
                .GetByIdAsync(request.BedId);
            if (bed == null) return new ResultDto<bool> { Data = false, Success = false, Message = "Bed not found" };
            await _unitOfWork.Beds.DeleteAsync(bed);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new ResultDto<bool> { Data = true, Success = true, Message = "Bed deleted successfully" };
        }
    }

    public record UpdateBedCommand(string BedId, string BedNumber, BedStatus Status) : IRequest<ResultDto<bool>>;

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
            bed.BedNumber = request.BedNumber;
            bed.Status = request.Status;
            await _unitOfWork.Beds.UpdateAsync(bed);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new ResultDto<bool> { Data = true, Success = true, Message = "Bed updated successfully" };
        }
    }
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