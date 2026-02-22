using MediatR;
using Cortexa.Domain.Enums;
using Cortexa.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Patients.Commands
{
    public class DischargePatientCommand : IRequest<bool>
    {
        public string AdmissionId { get; set; } = string.Empty;
        public DateTime DischargeDate { get; set; }
        public string DischargeSummary { get; set; } = string.Empty;
        public DischargeDisposition Disposition { get; set; }
    }

    public class DischargePatientCommandHandler : IRequestHandler<DischargePatientCommand, bool>
    {
        private readonly IAdmissionRepository _admissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DischargePatientCommandHandler(IAdmissionRepository admissionRepository, IUnitOfWork unitOfWork)
        {
            _admissionRepository = admissionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DischargePatientCommand request, CancellationToken cancellationToken)
        {
            var admission = await _admissionRepository.GetByIdAsync(request.AdmissionId);
            if (admission == null) return false;

            admission.DischargeDate = request.DischargeDate;
            admission.DischargeSummary = request.DischargeSummary;
            admission.DischargeDisposition = request.Disposition;
            admission.Status = AdmissionStatus.Discharged;

            await _admissionRepository.UpdateAsync(admission);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
