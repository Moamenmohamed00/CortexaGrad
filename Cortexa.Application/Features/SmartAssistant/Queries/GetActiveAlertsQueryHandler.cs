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
            // جلب التنبيهات بناءً على AdmissionId أو PatientId
            var alerts = request.AdmissionId != null
                ? await _aiRepository.GetAlertsByAdmissionIdAsync(request.AdmissionId)
                : await _aiRepository.GetAlertsByPatientIdAsync(request.PatientId!);

            // فلترة التنبيهات النشطة فقط (بافتراض أن لديك Status مثل Active)
            // وتحويلها إلى Dto
            var activeAlerts = alerts
                .Where(a => a.Status == AlertStatus.Active) // تأكد من اسم الحالة في الـ Enum الخاص بك
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