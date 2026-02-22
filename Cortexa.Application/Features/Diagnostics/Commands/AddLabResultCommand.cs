using MediatR;
using Cortexa.Domain.Entities.Diagnostics;
using Cortexa.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Diagnostics.Commands
{
    public class AddLabResultCommand : IRequest<string>
    {
        public string LabOrderId { get; set; } = string.Empty;
        public string Parameter { get; set; } = string.Empty;
        public float Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string? ReferenceRange { get; set; }
        public DateTime SampleDate { get; set; }
        public string NurseId { get; set; } = string.Empty;
    }

    public class AddLabResultCommandHandler : IRequestHandler<AddLabResultCommand, string>
    {
        private readonly ILabRepository _labRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddLabResultCommandHandler(ILabRepository labRepository, IUnitOfWork unitOfWork)
        {
            _labRepository = labRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(AddLabResultCommand request, CancellationToken cancellationToken)
        {
            var entity = new LabResult
            {
                LabOrderId = request.LabOrderId,
                Parameter = request.Parameter,
                Value = request.Value,
                Unit = request.Unit,
                ReferenceRange = request.ReferenceRange,
                SampleDate = request.SampleDate,
                NurseId = request.NurseId
            };

            await _labRepository.AddResultAsync(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
