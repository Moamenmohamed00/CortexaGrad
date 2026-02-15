using MediatR;
using AutoMapper;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.ClinicalData.Commands
{
    public class RecordVitalsCommand : IRequest<string>
    {
        public string AdmissionId { get; set; } = string.Empty;
        public float Temperature { get; set; }
        public int BP_Systolic { get; set; }
        public int BP_Diastolic { get; set; }
        public int HeartRate { get; set; }
        public int RespRate { get; set; }
        public int PulseOxy { get; set; }
        public DateTime RecordedAt { get; set; }
        public string NurseId { get; set; } = string.Empty;
    }

    public class RecordVitalsCommandHandler : IRequestHandler<RecordVitalsCommand, string>
    {
        private readonly IClinicalRepository _clinicalRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RecordVitalsCommandHandler(IClinicalRepository clinicalRepository, IUnitOfWork unitOfWork)
        {
            _clinicalRepository = clinicalRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(RecordVitalsCommand request, CancellationToken cancellationToken)
        {
            var entity = new VitalSigns
            {
                AdmissionId = request.AdmissionId,
                Temperature = request.Temperature,
                BP_Systolic = request.BP_Systolic,
                BP_Diastolic = request.BP_Diastolic,
                HeartRate = request.HeartRate,
                RespRate = request.RespRate,
                PulseOxy = request.PulseOxy,
                RecordedAt = request.RecordedAt,
                NurseId = request.NurseId
            };

            // Using generic AddAsync if possible, but I added specific one too. 
            // Since IClinicalRepository inherits IGenericRepository<VitalSigns>, AddAsync(VitalSigns) is available.
            // But specifically I added AddVitalSignsAsync. I'll use AddAsync if available from Generic, 
            // but wait, IClinicalRepository inherits IGenericRepository<VitalSigns>.
            // So AddAsync(VitalSigns) is there.
            await _clinicalRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
