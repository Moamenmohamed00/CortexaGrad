using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.ClinicalData.Commands.DeleteCommands
{
    public record DeleteMedicationCommand(
        string AdmissionId, 
        string Id
        ) : IRequest<bool>;

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
            var admission = await _unitOfWork.Admissions.GetByIdAsync(request.AdmissionId);

            if (entity == null || admission==null) return false;
            if (entity.AdmissionId != admission.Id) return false;

            await _unitOfWork.Medications.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }

  
}

