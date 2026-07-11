using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.ClinicalData.Commands.DeleteCommands
{
    public record DeleteInterventionProcedureCommand(
        string AdmissionId, string Id) : IRequest<bool>;

    public class DeleteInterventionProcedureCommandHandler : IRequestHandler<DeleteInterventionProcedureCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInterventionProcedureCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteInterventionProcedureCommand request, CancellationToken ct)
        {
            var entity = await _unitOfWork.InterventionProcedures.GetByIdAsync(request.Id);
            var admission = await _unitOfWork.Admissions.GetByIdAsync(request.AdmissionId);

            if (entity == null || admission==null) return false;
            if (entity.AdmissionId != admission.Id) return false;

            await _unitOfWork.InterventionProcedures.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }

   
}

