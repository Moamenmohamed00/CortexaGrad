using MediatR;

namespace Cortexa.Application.Features.SmartAssistant.Commands
{
    // 1. تعريف الأمر (البيانات المطلوبة للإلغاء)
    public class OverrideAlertCommand : IRequest<bool>
    {
        public string AlertId { get; set; } = string.Empty;
        public string DoctorId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string? ProcedureId { get; set; } // اختياري
    }
}