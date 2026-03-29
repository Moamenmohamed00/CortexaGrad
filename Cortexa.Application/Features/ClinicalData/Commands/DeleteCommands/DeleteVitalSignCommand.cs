using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.ClinicalData.Commands.DeleteCommands
{
    public record DeleteVitalSignCommand(string Id) : IRequest<bool>;

    public class DeleteVitalSignCommandHandler : IRequestHandler<DeleteVitalSignCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteVitalSignCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteVitalSignCommand request, CancellationToken ct)
        {
            var entity = await _unitOfWork.VitalSigns.GetByIdAsync(request.Id);
            if (entity == null) return false;

            await _unitOfWork.VitalSigns.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }

    
}

