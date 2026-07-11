using MediatR;
using Cortexa.Domain.Entities.Diagnostics;
using Cortexa.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Diagnostics.Commands
{
    public class CreateLabOrderCommand : IRequest<string>
    {
        public string AdmissionId { get; set; } = string.Empty;
        public string TestName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string DoctorId { get; set; } = string.Empty;
    }

    public class CreateLabOrderCommandHandler : IRequestHandler<CreateLabOrderCommand, string>
    {
        private readonly ILabRepository _labRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateLabOrderCommandHandler(ILabRepository labRepository, IUnitOfWork unitOfWork)
        {
            _labRepository = labRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateLabOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = new LabOrder
            {
                AdmissionId = request.AdmissionId,
                TestName = request.TestName,
                OrderDate = request.OrderDate,
                DoctorId = request.DoctorId
            };

            await _labRepository.AddLabOrderAsync(entity);
            // Note: AddLabOrderAsync in interface likely returns Task. created in previous step.
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
