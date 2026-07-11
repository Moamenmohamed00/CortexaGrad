using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.Admin.Commands
{
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
}