using MediatR;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Domain.Enums;
using Cortexa.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Cortexa.Application.Interfaces.Repositories.Clinical;

namespace Cortexa.Application.Features.ClinicalData.Commands
{
    public class PrescribeMedicationCommand : IRequest<string>
    {
        public string AdmissionId { get; set; } = string.Empty;
        public string DrugName { get; set; } = string.Empty;
        public int Dose { get; set; }
        public string DoseUnit { get; set; } = string.Empty;
        public int Frequency { get; set; }
        public MedicationRoute Route { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DoctorId { get; set; } = string.Empty;
    }

    public class PrescribeMedicationCommandHandler : IRequestHandler<PrescribeMedicationCommand, string>
    {
        private readonly IMedicationRepository _medicationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PrescribeMedicationCommandHandler(IMedicationRepository medicationRepository, IUnitOfWork unitOfWork)
        {
            _medicationRepository = medicationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(PrescribeMedicationCommand request, CancellationToken cancellationToken)
        {
            var entity = new Medications
            {
                AdmissionId = request.AdmissionId,
                DrugName = request.DrugName,
                Dose = request.Dose,
                DoseUnit = request.DoseUnit,
                Frequency = request.Frequency,
                Route = request.Route,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                DoctorId = request.DoctorId
            };

            await _medicationRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
