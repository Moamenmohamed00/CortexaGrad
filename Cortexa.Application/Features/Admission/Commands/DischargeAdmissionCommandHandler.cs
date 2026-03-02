using Cortexa.Application.Common.Interfaces;
using Cortexa.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.Admission.Commands
{
    public record DischargeAdmissionCommand(
       string AdmissionId,
       DateTime DischargeDate,
       string DischargeSummary,
       DischargeDisposition Disposition
   ) : IRequest<bool>;
    public class DischargeAdmissionCommandHandler
        : IRequestHandler<DischargeAdmissionCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public DischargeAdmissionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(
            DischargeAdmissionCommand request,
            CancellationToken cancellationToken)
        {
            var admission = await _context.Admissions
                .FindAsync(new object[] { request.AdmissionId }, cancellationToken);

            if (admission == null)
                return false;

            admission.DischargeDate = request.DischargeDate;
            admission.DischargeSummary = request.DischargeSummary;
            admission.DischargeDisposition = request.Disposition;
            admission.Status = AdmissionStatus.Discharged;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
