using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.AI;
using Cortexa.Domain.Enums;

namespace Cortexa.Application.Features.SmartAssistant.Commands
{
    // 2. منفذ الأمر (الـ Handler)
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
            // 1. جلب التنبيه من الداتا بيز
            var alert = await _aiRepository.GetByIdAsync(request.AlertId);

            if (alert == null) return false;

            // 2. تعديل حالة التنبيه
            alert.Status = AlertStatus.Resolved; // أو أي حالة تعبر عن الإلغاء في AlertStatus

            // 3. إنشاء سجل الإلغاء (Audit Log)
            var overrideLog = new AlertOverrideLog
            {
                AlertId = request.AlertId,
                DoctorId = request.DoctorId,
                Reason = request.Reason,
                ProcedureId = request.ProcedureId,
                OverrideTime = DateTime.UtcNow
            };

            alert.AlertOverrideLogs.Add(overrideLog);

            // 4. حفظ التعديلات في الداتا بيز باستخدام الـ Repositories لديك
           await _aiRepository.UpdateAsync(alert);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}