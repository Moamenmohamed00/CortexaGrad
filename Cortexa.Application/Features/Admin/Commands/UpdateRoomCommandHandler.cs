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
}