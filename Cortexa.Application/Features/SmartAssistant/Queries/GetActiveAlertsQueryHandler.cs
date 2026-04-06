using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Cortexa.Application.Dtos.AI;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Features.SmartAssistant.Queries
{
    // 2. منفذ الاستعلام (الـ Handler)
    public class GetActiveAlertsQueryHandler : IRequestHandler<GetActiveAlertsQuery, IEnumerable<AlertDto>>
    {
        private readonly IAIRepository _aiRepository;

        public GetActiveAlertsQueryHandler(IAIRepository aiRepository)
        {
            _aiRepository = aiRepository;
        }

        public async Task<IEnumerable<AlertDto>> Handle(GetActiveAlertsQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<Domain.Entities.AI.Alert> alerts;

            if (request.AdmissionId != null)
                alerts = await _aiRepository.GetAlertsByAdmissionIdAsync(request.AdmissionId);
            else if (request.PatientId != null)
                alerts = await _aiRepository.GetAlertsByPatientIdAsync(request.PatientId);
            else
                alerts = await _aiRepository.GetAllActiveAlertsAsync(); // no filter → all active

            return alerts
                .Where(a => a.Status == AlertStatus.Active)
                .Select(a => new AlertDto(
                    a.Id,
                    a.AlertMessage,
                    a.Severity,
                    a.GeneratedAt,
                    a.Status,
                    a.AdmissionId
                )).ToList();
        }
    }
}