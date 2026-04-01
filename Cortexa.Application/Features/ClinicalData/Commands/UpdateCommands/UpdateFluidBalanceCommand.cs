using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Enums;
using MediatR;

namespace Cortexa.Application.Features.ClinicalData.Commands.UpdateCommands
{
    public class UpdateFluidBalanceCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public FluidBalanceCategory Category { get; set; }
        public FluidType Type { get; set; }
        public int Amount_ML { get; set; }
        public DateTime RecordedAt { get; set; }
    }
    public class UpdateFluidBalanceCommandHandler : IRequestHandler<UpdateFluidBalanceCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateFluidBalanceCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(UpdateFluidBalanceCommand request, CancellationToken ct)
        {
            var entity = await _unitOfWork.FluidBalances.GetByIdAsync(request.Id);
            if (entity == null) return false;

            entity.Category = request.Category;
            entity.Type = request.Type;
            entity.Amount_ML = request.Amount_ML;
            entity.RecordedAt = request.RecordedAt;

            await _unitOfWork.FluidBalances.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }

  
}

