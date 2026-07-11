using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.ClinicalData.Commands.DeleteCommands
{
    public record DeleteFluidBalanceCommand(
        string AdmissionId,
        string Id
        ) : IRequest<bool>;

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
            var admission = await _unitOfWork.Admissions.GetByIdAsync(request.AdmissionId);

            if (entity == null || admission==null) return false;
            if (entity.AdmissionId != admission.Id) return false;

            await _unitOfWork.FluidBalances.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }

    
}

