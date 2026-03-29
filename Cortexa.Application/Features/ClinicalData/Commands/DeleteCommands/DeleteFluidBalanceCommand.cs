using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.ClinicalData.Commands.DeleteCommands
{
    public record DeleteFluidBalanceCommand(string Id) : IRequest<bool>;

    public class DeleteFluidBalanceCommandHandler : IRequestHandler<DeleteFluidBalanceCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFluidBalanceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteFluidBalanceCommand request, CancellationToken ct)
        {
            var entity = await _unitOfWork.FluidBalances.GetByIdAsync(request.Id);
            if (entity == null) return false;

            await _unitOfWork.FluidBalances.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }

    
}

