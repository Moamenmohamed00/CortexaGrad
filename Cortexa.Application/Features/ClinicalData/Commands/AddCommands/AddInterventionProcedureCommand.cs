using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Repositories.Clinical;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Features.ClinicalData.Commands.AddCommands
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
        private readonly IUnitOfWork _unitOfWork;

        public AddInterventionProcedureCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<string> Handle(
            AddInterventionProcedureCommand request,
            CancellationToken cancellationToken)
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

            await _unitOfWork.InterventionProcedures.AddAsync(entity,cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
