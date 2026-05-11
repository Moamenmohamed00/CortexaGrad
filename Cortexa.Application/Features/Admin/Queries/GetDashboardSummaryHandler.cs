using Cortexa.Application.Common.Interfaces;
using Cortexa.Application.Dtos.Admin;
using Cortexa.Application.Dtos.AI;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.AI;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Entities.Infrastructure;
using Cortexa.Domain.Enums;
using MediatR;
using Cortexa.Application.Dtos.AuditLog;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.Admin.Queries
{



    // Cortexa.Application/Features/Admin/Queries/GetDashboardSummaryQuery.cs
    public record GetDashboardSummaryQuery : IRequest<DashboardSummaryDto>;

    public class GetDashboardSummaryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTime _dateTime; // الـ Service اللي عندك للوقت

        public GetDashboardSummaryHandler(IUnitOfWork unitOfWork, IDateTime dateTime)
        {
            _unitOfWork = unitOfWork;
            _dateTime = dateTime;
        }

        public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
            // 1. حساب نسبة إشغال الأسرة
            var totalBeds = await _unitOfWork.Beds.GetAllAsync();
            var activeAdmissions = await _unitOfWork.Admissions.FindAsync(a => a.Status == AdmissionStatus.Active);
            // 2. جلب التنبيهات الخطيرة غير المحلولة
            var highRiskAlerts = await _unitOfWork.AI.FindAsync(a => a.Status != AlertStatus.Resolved && a.Severity == AlertSeverity.High);

            // 3. إحصائيات الـ AI (RAG) لليوم
            var today = _dateTime.Now.Date;
            var ragQueriesToday = await _unitOfWork.Rags
                .FindAsync(q => q.CreatedAt >= today);

            // 4. آخر العمليات من الـ Audit Logs (أنت عندك AuditLog Entity فعلاً)
            var recentLogs = await _unitOfWork.AuditLogs
                .GetPagedAsync(1, 20, l => true, q => q.OrderByDescending(l => l.Timestamp));

            return new DashboardSummaryDto
            {
                TotalActivePatients = activeAdmissions.Count(),
                BedOccupancyPercentage = totalBeds.Count() > 0 ? (double)activeAdmissions.Count() / totalBeds.Count() * 100 : 0,
                HighRiskAlertsCount = highRiskAlerts.Count(),
                TotalRAGQueriesToday = ragQueriesToday.Count(),
                RecentSystemActivities = recentLogs.Items.Select(l => new RecentActivityDto
                {
                    Action = l.Type.ToString(),
                    UserId = l.UserId, 
                    Timestamp = l.Timestamp,
                    EntityName = l.EntityName
                }).ToList()
            };
        }
    }

    //// Cortexa.Application/Features/Admin/Queries/GetRAGPerformanceQuery.cs
    //public class GetRAGPerformanceHandler : IRequestHandler<GetRAGPerformanceQuery, List<RagQueryDto>>
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly IMapper _mapper;       

    //    public async Task<List<RagQueryDto>> Handle(GetRAGPerformanceQuery request, CancellationToken cancellationToken)
    //    {
    //        var ragQueries = await _unitOfWork.Rags
    //            .FindAsync(q => q.ScoreTrust < 0.5);
    //        return _mapper.Map<List<RagQueryDto>>(ragQueries);
    //    }
    //}

   

    

}
