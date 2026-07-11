using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.ClinicalData.Commands.DeleteCommands
{
    public record DeleteNursingNoteCommand(string AdmissionId, string Id) : IRequest<bool>;

    public class DeleteNursingNoteCommandHandler : IRequestHandler<DeleteNursingNoteCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteNursingNoteCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteNursingNoteCommand request, CancellationToken ct)
        {
            var entity = await _unitOfWork.NursingNotes.GetByIdAsync(request.Id);
            var admission = await _unitOfWork.Admissions.GetByIdAsync(request.AdmissionId);

            if (entity == null || admission==null) return false;
            if (entity.AdmissionId != admission.Id) return false;

            await _unitOfWork.NursingNotes.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }

  
}

