using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Repositories.Clinical;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.ClinicalData.Commands
{
    public class AddInterventionProcedureCommand : IRequest<string>
    {
        public CareInterventionType Type { get; set; }
        public int Size { get; set; }
        public DateTime InsertionDate { get; set; }
        public DateTime? RemovalDate { get; set; }

        public string AdmissionId { get; set; } = string.Empty;
        public string NurseId { get; set; } = string.Empty;
    }

    public class AddInterventionProcedureCommandHandler
        : IRequestHandler<AddInterventionProcedureCommand, string>
    {
        private readonly IInterventionProcedureRepository _repo;
        private readonly IUnitOfWork _uow;

        public AddInterventionProcedureCommandHandler(
            IInterventionProcedureRepository repo,
            IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<string> Handle(
            AddInterventionProcedureCommand request,
            CancellationToken ct)
        {
            var entity = new InterventionProcedure
            {
                Type = request.Type,
                Size = request.Size,
                InsertionDate = request.InsertionDate,
                RemovalDate = request.RemovalDate,
                AdmissionId = request.AdmissionId,
                NurseId = request.NurseId
            };

            await _repo.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);

            return entity.Id;
        }
    }
}
