using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.ClinicalData.Commands.DeleteCommands
{
    public record DeleteInterventionProcedureCommand(string Id) : IRequest<bool>;

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
            if (entity == null) return false;

            await _unitOfWork.InterventionProcedures.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }

   
}

