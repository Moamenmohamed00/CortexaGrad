using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.ClinicalData.Commands.UpdateCommands
{
    public class UpdateNursingNoteCommand : IRequest<bool>
    {
        public string AdmissionId { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string NoteText { get; set; } = string.Empty;
        public DateTime NoteDateTime { get; set; }
    }
    public class UpdateNursingNoteCommandHandler : IRequestHandler<UpdateNursingNoteCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateNursingNoteCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(UpdateNursingNoteCommand request, CancellationToken ct)
        {
            var entity = await _unitOfWork.NursingNotes.GetByIdAsync(request.Id);
            var admission = await _unitOfWork.Admissions.GetByIdAsync(request.AdmissionId);

            if (entity == null || admission==null) return false;
            if (entity.AdmissionId != admission.Id) return false;

            entity.NoteText = request.NoteText;
            entity.NoteDateTime = request.NoteDateTime;

            await _unitOfWork.NursingNotes.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }


}

