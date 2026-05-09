using AutoMapper;
using Cortexa.Application.Dtos.AuditLog;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.Admin.Queries
{

    public class GetSystemAuditLogsQuery : IRequest<PagedResult<AuditLogResponseDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? TableName { get; set; } // فلترة حسب الجدول (مثلاً Medications)
        public string? UserId { get; set; } // فلترة حسب الطبيب أو الممرض
    }
    public class GetSystemAuditLogsHandler : IRequestHandler<GetSystemAuditLogsQuery, PagedResult<AuditLogResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSystemAuditLogsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<AuditLogResponseDto>> Handle(GetSystemAuditLogsQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber <= 0)
                request.PageNumber = 1;

            var logs = await _unitOfWork.AuditLogs
                .GetPagedAsync(request.PageNumber, request.PageSize,
                    l => (string.IsNullOrEmpty(request.TableName) || l.EntityName == request.TableName) &&
                         (string.IsNullOrEmpty(request.UserId) || l.UserId == request.UserId),
                    q => q.OrderByDescending(l => l.Timestamp));

            // التحويل لـ DTO
            return new PagedResult<AuditLogResponseDto>(
                 request.PageNumber,
                request.PageSize,
                logs.TotalCount,
                logs.Items.Select(l => new AuditLogResponseDto
                {
                    Id = l.Id,
                    EntityId = l.EntityId,
                    EntityName = l.EntityName,
                    Type = l.Type,
                    OldValue = l.OldValue,
                    NewValue = l.NewValue,
                    AffectedColumns = l.AffectedColumns,
                    Timestamp = l.Timestamp,
                    UserId = l.UserId
                }).ToList());
        }
    }
}
