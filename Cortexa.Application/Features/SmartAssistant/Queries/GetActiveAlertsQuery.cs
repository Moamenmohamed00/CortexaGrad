using MediatR;
using Cortexa.Application.Dtos.AI;
using System.Collections.Generic;

namespace Cortexa.Application.Features.SmartAssistant.Queries
{
    // 1. تعريف الاستعلام (البيانات المطلوبة للبحث)
    // نطلب إرجاع قائمة من AlertDto
    public class GetActiveAlertsQuery : IRequest<IEnumerable<AlertDto>>
    {
        public string? PatientId { get; set; }
        public string? AdmissionId { get; set; }
    }
}