using MediatR;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.ClinicalData.Commands
{
    public class AddNursingNoteCommand : IRequest<string>
    {
        public string AdmissionId { get; set; } = string.Empty;
        public string NoteText { get; set; } = string.Empty;
        public DateTime NoteDateTime { get; set; }
        public string NurseId { get; set; } = string.Empty;
    }

    public class AddNursingNoteCommandHandler : IRequestHandler<AddNursingNoteCommand, string>
    {
        private readonly IClinicalRepository _clinicalRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddNursingNoteCommandHandler(IClinicalRepository clinicalRepository, IUnitOfWork unitOfWork)
        {
            _clinicalRepository = clinicalRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(AddNursingNoteCommand request, CancellationToken cancellationToken)
        {
            var entity = new NursingNotes
            {
                AdmissionId = request.AdmissionId,
                NoteText = request.NoteText,
                NoteDateTime = request.NoteDateTime,
                NurseId = request.NurseId
            };

            await _clinicalRepository.AddNursingNoteAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
