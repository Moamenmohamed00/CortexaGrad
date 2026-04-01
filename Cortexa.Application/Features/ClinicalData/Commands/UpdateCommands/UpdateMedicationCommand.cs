using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Enums;
using MediatR;

namespace Cortexa.Application.Features.ClinicalData.Commands.UpdateCommands
{
    public class UpdateMedicationCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public string DrugName { get; set; } = string.Empty;
        public int Dose { get; set; }
        public string DoseUnit { get; set; } = string.Empty;
        public int Frequency { get; set; }
        public MedicationRoute Route { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class UpdateMedicationCommandHandler : IRequestHandler<UpdateMedicationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateMedicationCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(UpdateMedicationCommand request, CancellationToken ct)
        {
            var entity = await _unitOfWork.Medications.GetByIdAsync(request.Id);
            if (entity == null) return false;

            entity.DrugName = request.DrugName;
            entity.Dose = request.Dose;
            entity.DoseUnit = request.DoseUnit;
            entity.Frequency = request.Frequency;
            entity.Route = request.Route;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;

            await _unitOfWork.Medications.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }

  
}

