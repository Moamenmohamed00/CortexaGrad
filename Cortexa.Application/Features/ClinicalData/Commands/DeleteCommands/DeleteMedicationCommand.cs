using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.ClinicalData.Commands.DeleteCommands
{
    public record DeleteMedicationCommand(string Id) : IRequest<bool>;

    public class DeleteMedicationCommandHandler : IRequestHandler<DeleteMedicationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMedicationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteMedicationCommand request, CancellationToken ct)
        {
            var entity = await _unitOfWork.Medications.GetByIdAsync(request.Id);
            if (entity == null) return false;

            await _unitOfWork.Medications.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }

  
}

