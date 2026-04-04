using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Services;
using Cortexa.Domain.Entities.AI;
using Cortexa.Domain.Enums;
using Cortexa.Domain.Services;
using MediatR;
using System.Threading;

namespace Cortexa.Application.Features.ClinicalData.Commands.UpdateCommands
{
    public class UpdateVitalsCommand : IRequest<bool> // ✅ رجع ID أو Error handling
    {
        public string Id { get; set; } = string.Empty;
        public string AdmissionId { get; set; } = string.Empty;

        public DateTime RecordedAt { get; set; }

        public float Temperature { get; set; }
        public int HeartRate { get; set; }
        public int RespRate { get; set; }

        public int BpSystolic { get; set; }
        public int BpDiastolic { get; set; }

        public int PulseOxy { get; set; }
        public int Cvp { get; set; }
        public decimal InsulinGiven { get; set; }
        public int GcsEye { get; set; }
        public int GcsVerbal { get; set; }
        public int GcsMotor { get; set; }

        public bool SupplementalOxygen { get; set; }
        public ConsciousnessLevel ConsciousnessLevel { get; set; }
    }

    public class UpdateVitalsCommandHandler : IRequestHandler<UpdateVitalsCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public UpdateVitalsCommandHandler(IUnitOfWork unitOfWork, INotificationService notificationService) 
        {
            _unitOfWork = unitOfWork; 
            _notificationService = notificationService;
        }

        public async Task<bool> Handle(UpdateVitalsCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.VitalSigns.GetByIdAsync(request.Id);
            var admission = await _unitOfWork.Admissions.GetByIdAsync(request.AdmissionId);

            if (entity == null || admission==null) return false;
            if (entity.AdmissionId != admission.Id) return false;

            entity.Temperature = request.Temperature;
            entity.BpSystolic = request.BpSystolic;
            entity.BpDiastolic = request.BpDiastolic;
            entity.HeartRate = request.HeartRate;
            entity.RespRate = request.RespRate;
            entity.PulseOxy = request.PulseOxy;
            entity.SupplementalOxygen = request.SupplementalOxygen;
            entity.ConsciousnessLevel = request.ConsciousnessLevel;
            entity.RecordedAt = request.RecordedAt;

            // ── Calculate NEWS score ─────────────────────────────────
            var (newsScore, riskLevel) = NewsCalculator.Calculate(entity);
            entity.NewsScore = newsScore;
            entity.NewsRiskLevel = riskLevel;

            await _unitOfWork.VitalSigns.UpdateAsync(entity);

            // ── Auto-generate alert for Medium / High risk ───────────
            if (riskLevel >= NewsRiskLevel.Medium)
            {
                var severity = riskLevel == NewsRiskLevel.High
                    ? AlertSeverity.Critical
                    : AlertSeverity.High;

                var alert = new Alert
                {
                    AdmissionId = request.Id,
                    AlertMessage = $"NEWS Score: {newsScore} — {riskLevel} clinical risk. " +
                                   $"Immediate assessment required.",
                    Severity = severity,
                    GeneratedAt = DateTime.UtcNow,
                    Status = AlertStatus.Active
                };

                await _unitOfWork.AI.AddAsync(alert, cancellationToken);

                // ── Send real-time SignalR notification ───────────────
                await _notificationService.SendRealTimeAlertAsync(
                    request.Id,
                    $"NEWS-{riskLevel}",
                    alert.AlertMessage,
                    cancellationToken);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}

