using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.ClinicalData.Commands.UpdateCommands
{
    public class UpdateCaseHistoryCommand : IRequest<bool>
        {
            public string Id { get; set; } = string.Empty;
            public string Complaint { get; set; } = string.Empty;
            public string PresentIllness { get; set; } = string.Empty;
            public string? ChronicDisease { get; set; }
            public string? GeneticDisease { get; set; }
            public string? MaritalHistory { get; set; }
            public string? SpecialHabits { get; set; }
            public string? ClinicalNotes { get; set; }
        }
    public class UpdateCaseHistoryCommandHandler
           : IRequestHandler<UpdateCaseHistoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCaseHistoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateCaseHistoryCommand request, CancellationToken ct)
        {
            var entity = await _unitOfWork.CaseHistories.GetByIdAsync(request.Id);

            if (entity == null) return false;

            entity.Complaint = request.Complaint;
            entity.PresentIllness = request.PresentIllness;
            entity.ChronicDisease = request.ChronicDisease;
            entity.GeneticDisease = request.GeneticDisease;
            entity.MaritalHistory = request.MaritalHistory;
            entity.SpecialHabits = request.SpecialHabits;
            entity.ClinicalNotes = request.ClinicalNotes;

            await _unitOfWork.CaseHistories.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }
    }

    
}

