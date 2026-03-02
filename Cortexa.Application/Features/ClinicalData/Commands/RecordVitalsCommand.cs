using MediatR;
using AutoMapper;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Cortexa.Application.Interfaces.Repositories.Clinical;

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
        private readonly IVitalSignsRepository _vitalSignsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RecordVitalsCommandHandler( IVitalSignsRepository vitalSignsRepository, IUnitOfWork unitOfWork)
        {
            _vitalSignsRepository = vitalSignsRepository;
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


            await _vitalSignsRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
