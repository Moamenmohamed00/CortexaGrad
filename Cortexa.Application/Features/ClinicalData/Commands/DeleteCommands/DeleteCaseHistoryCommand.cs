using Cortexa.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.ClinicalData.Commands.DeleteCommands
{

    public record DeleteCaseHistoryCommand(
        string AdmissionId,
        string Id
        ) : IRequest<bool>;

    public class DeleteCaseHistoryCommandHandler
        : IRequestHandler<DeleteCaseHistoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCaseHistoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCaseHistoryCommand request, CancellationToken ct)
        {
            var entity = await _unitOfWork.CaseHistories.GetByIdAsync(request.Id);

            var admission = await _unitOfWork.Admissions.GetByIdAsync(request.AdmissionId);

            if (entity == null || admission==null) return false;
            if (entity.AdmissionId != admission.Id) return false;

            await _unitOfWork.CaseHistories.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }
    }

   
}

