using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Enums;
using MediatR;

namespace Cortexa.Application.Features.ClinicalData.Commands.UpdateCommands
{
    public class UpdateInterventionProcedureCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public CareInterventionType Type { get; set; }
        public int Size { get; set; }
        public DateTime InsertionDate { get; set; }
        public DateTime? RemovalDate { get; set; }
    }
    public class UpdateInterventionProcedureCommandHandler : IRequestHandler<UpdateInterventionProcedureCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateInterventionProcedureCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(UpdateInterventionProcedureCommand request, CancellationToken ct)
        {
            var entity = await _unitOfWork.InterventionProcedures.GetByIdAsync(request.Id);
            if (entity == null) return false;

            entity.Type = request.Type;
            entity.Size = request.Size;
            entity.InsertionDate = request.InsertionDate;
            entity.RemovalDate = request.RemovalDate;

            await _unitOfWork.InterventionProcedures.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }

   
}

