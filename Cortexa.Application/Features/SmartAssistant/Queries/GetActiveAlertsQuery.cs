using Cortexa.Application.Dtos.AI;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Enums;
using MediatR;

namespace Cortexa.Application.Features.SmartAssistant.Queries
{
    public class GetActiveAlertsQuery : IRequest<IEnumerable<AlertDto>>
    {
        public string? PatientId { get; set; }
        public string? AdmissionId { get; set; }
    }

    public class GetActiveAlertsQueryHandler : IRequestHandler<GetActiveAlertsQuery, IEnumerable<AlertDto>>
    {
        private readonly IAIRepository _aiRepository;

        public GetActiveAlertsQueryHandler(IAIRepository aiRepository)
        {
            _aiRepository = aiRepository;
        }

        public async Task<IEnumerable<AlertDto>> Handle(GetActiveAlertsQuery request, CancellationToken cancellationToken)
        {
            var alerts = request.AdmissionId != null
                ? await _aiRepository.GetAlertsByAdmissionIdAsync(request.AdmissionId)
                : await _aiRepository.GetAlertsByPatientIdAsync(request.PatientId!);


            var activeAlerts = alerts
                .Where(a => a.Status == AlertStatus.Active)
                .Select(a => new AlertDto(
                    a.Id,
                    a.AlertMessage,
                    a.Severity,
                    a.GeneratedAt,
                    a.Status,
                    a.AdmissionId
                )).ToList();

            return activeAlerts;
        }
    }
}
