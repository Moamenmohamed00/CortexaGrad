using MediatR;
using Cortexa.Domain.Entities.Clinical;
using Cortexa.Domain.Enums;
using Cortexa.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.ClinicalData.Commands
{
    public class AddFluidBalanceCommand : IRequest<string>
    {
        public string AdmissionId { get; set; } = string.Empty;
        public FluidBalanceCategory Category { get; set; }
        public FluidType Type { get; set; }
        public int Amount_ML { get; set; }
        public DateTime RecordedAt { get; set; }
        public string NurseId { get; set; } = string.Empty;
    }

    public class AddFluidBalanceCommandHandler : IRequestHandler<AddFluidBalanceCommand, string>
    {
        private readonly IClinicalRepository _clinicalRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddFluidBalanceCommandHandler(IClinicalRepository clinicalRepository, IUnitOfWork unitOfWork)
        {
            _clinicalRepository = clinicalRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(AddFluidBalanceCommand request, CancellationToken cancellationToken)
        {
            var entity = new FluidBalance
            {
                AdmissionId = request.AdmissionId,
                Category = request.Category,
                Type = request.Type,
                Amount_ML = request.Amount_ML,
                RecordedAt = request.RecordedAt,
                NurseId = request.NurseId
            };

            await _clinicalRepository.AddFluidBalanceAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
