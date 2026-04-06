using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.AI;
using Cortexa.Domain.Enums;
using MediatR;

namespace Cortexa.Application.Features.SmartAssistant.Commands
{
    public class OverrideAlertCommand : IRequest<bool>
    {
        public string AlertId { get; set; } = string.Empty;
        public string DoctorId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string? ProcedureId { get; set; } 
    }
    public class OverrideAlertCommandHandler : IRequestHandler<OverrideAlertCommand, bool>
    {
        private readonly IAIRepository _aiRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OverrideAlertCommandHandler(IAIRepository aiRepository, IUnitOfWork unitOfWork)
        {
            _aiRepository = aiRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(OverrideAlertCommand request, CancellationToken cancellationToken)
        {
            var alert = await _aiRepository.GetByIdAsync(request.AlertId);

            if (alert == null) return false;

            alert.Status = AlertStatus.Resolved; 

            var overrideLog = new AlertOverrideLog
            {
                AlertId = request.AlertId,
                DoctorId = request.DoctorId,
                Reason = request.Reason,
                ProcedureId = request.ProcedureId,
                OverrideTime = DateTime.UtcNow
            };

            alert.AlertOverrideLogs.Add(overrideLog);

            await _aiRepository.UpdateAsync(alert);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
