using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Cortexa.Application.Dtos.AI;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Enums;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<AlertDto>> Handle(
    GetActiveAlertsQuery request,
    CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AI.GetAlertsQuery();

            query = query.Where(a => a.Status == AlertStatus.Active);

            if (!string.IsNullOrWhiteSpace(request.PatientId))
            {
                query = query.Where(a =>
                    a.Admission != null &&
                    a.Admission.PatientId == request.PatientId);
            }

            if (!string.IsNullOrWhiteSpace(request.AdmissionId))
            {
                query = query.Where(a =>
                    a.AdmissionId == request.AdmissionId);
            }

            return await query
                .OrderByDescending(a => a.GeneratedAt)
                .Select(a => new AlertDto(
                    a.Id,
                    a.Admission != null && a.Admission.Patient != null
                        ? a.Admission.Patient.Name
                        : "Unknown Patient",
                    a.AlertMessage,
                    a.Severity,
                    a.GeneratedAt,
                    a.Status,
                    a.AdmissionId
                ))
                .ToListAsync(cancellationToken);
        }
    }
}