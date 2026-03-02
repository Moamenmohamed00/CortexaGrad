using MediatR;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Domain.Entities.AI;
using Cortexa.Domain.Enums;
using Cortexa.Domain.Services;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Repositories.Clinical;
using Cortexa.Application.Interfaces.Services;
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
        public bool SupplementalOxygen { get; set; }
        public ConsciousnessLevel ConsciousnessLevel { get; set; }
        public DateTime RecordedAt { get; set; }
        public string NurseId { get; set; } = string.Empty;
    }

    public class RecordVitalsCommandHandler : IRequestHandler<RecordVitalsCommand, string>
    {
        private readonly IVitalSignsRepository _vitalSignsRepository;
        private readonly IAIRepository _aiRepository;
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;

        public RecordVitalsCommandHandler(
            IVitalSignsRepository vitalSignsRepository,
            IAIRepository aiRepository,
            INotificationService notificationService,
            IUnitOfWork unitOfWork)
        {
            _vitalSignsRepository = vitalSignsRepository;
            _aiRepository = aiRepository;
            _notificationService = notificationService;
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
                SupplementalOxygen = request.SupplementalOxygen,
                ConsciousnessLevel = request.ConsciousnessLevel,
                RecordedAt = request.RecordedAt,
                NurseId = request.NurseId
            };

            // ── Calculate NEWS score ─────────────────────────────────
            var (newsScore, riskLevel) = NewsCalculator.Calculate(entity);
            entity.NewsScore = newsScore;
            entity.NewsRiskLevel = riskLevel;

            await _vitalSignsRepository.AddAsync(entity, cancellationToken);

            // ── Auto-generate alert for Medium / High risk ───────────
            if (riskLevel >= NewsRiskLevel.Medium)
            {
                var severity = riskLevel == NewsRiskLevel.High
                    ? AlertSeverity.Critical
                    : AlertSeverity.High;

                var alert = new Alert
                {
                    AdmissionId = request.AdmissionId,
                    AlertMessage = $"NEWS Score: {newsScore} — {riskLevel} clinical risk. " +
                                   $"Immediate assessment required.",
                    Severity = severity,
                    GeneratedAt = DateTime.UtcNow,
                    Status = AlertStatus.Active
                };

                await _aiRepository.AddAsync(alert, cancellationToken);

                // ── Send real-time SignalR notification ───────────────
                await _notificationService.SendRealTimeAlertAsync(
                    request.AdmissionId,
                    $"NEWS-{riskLevel}",
                    alert.AlertMessage,
                    cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}

