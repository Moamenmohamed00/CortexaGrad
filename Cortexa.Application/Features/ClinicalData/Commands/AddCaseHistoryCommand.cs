using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Repositories.Clinical;
using Cortexa.Domain.Entities.Clinical;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.ClinicalData.Commands
{
    public class AddCaseHistoryCommand : IRequest<string>
    {
        public string AdmissionId { get; set; } = string.Empty;
        public string Complaint { get; set; } = string.Empty;
        public string PresentIllness { get; set; } = string.Empty;
        public string? ChronicDisease { get; set; }
        public string? GeneticDisease { get; set; }
        public string? MaritalHistory { get; set; }
        public string? SpecialHabits { get; set; }
        public string? ClinicalNotes { get; set; }
        public string DoctorId { get; set; } = string.Empty;
    }

    public class AddCaseHistoryCommandHandler
        : IRequestHandler<AddCaseHistoryCommand, string>
    {
        private readonly ICaseHistoryRepository _repo;
        private readonly IUnitOfWork _uow;

        public AddCaseHistoryCommandHandler(
            ICaseHistoryRepository repo,
            IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<string> Handle(
            AddCaseHistoryCommand request,
            CancellationToken ct)
        {
            var entity = new CaseHistory
            {
                AdmissionId = request.AdmissionId,
                Complaint = request.Complaint,
                PresentIllness = request.PresentIllness,
                ChronicDisease = request.ChronicDisease,
                GeneticDisease = request.GeneticDisease,
                MaritalHistory = request.MaritalHistory,
                SpecialHabits = request.SpecialHabits,
                ClinicalNotes = request.ClinicalNotes,
                DoctorId = request.DoctorId
            };

            await _repo.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            return entity.Id;
        }
    }
}
