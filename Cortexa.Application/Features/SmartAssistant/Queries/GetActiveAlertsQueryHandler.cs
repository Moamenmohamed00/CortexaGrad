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
        private readonly IUnitOfWork _unitOfWork;

        public GetActiveAlertsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AlertDto>> Handle(GetActiveAlertsQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<Domain.Entities.AI.Alert> alerts;

            if (request.AdmissionId != null)
                alerts = await _unitOfWork.AI.GetAlertsByAdmissionIdAsync(request.AdmissionId);
            else if (request.PatientId != null)
                alerts = await _unitOfWork.AI.GetAlertsByPatientIdAsync(request.PatientId);
            else
                alerts = await _unitOfWork.AI.GetAllActiveAlertsAsync(); // no filter → all active

            if (request.AdmissionId == null || alerts == null)
                throw new Exception("AdmissionId Or PatientId is Null");

            string patientName = await _unitOfWork.Admissions
                .GetPatientNameByAdmissionIdAsync(request.AdmissionId);

            return alerts
                .Where(a => a.Status == AlertStatus.Active)
                .Select(a => new AlertDto(
                    a.Id,
                    patientName,
                    a.AlertMessage,
                    a.Severity,
                    a.GeneratedAt,
                    a.Status,
                    a.AdmissionId
                )).ToList();
        }
    }
}