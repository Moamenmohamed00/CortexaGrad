using Cortexa.Application.Interfaces.Repositories;
using MediatR;

namespace Cortexa.Application.Features.ClinicalData.Commands.DeleteCommands
{
    public record DeletePhysicalExaminationCommand(string Id) : IRequest<bool>;
    public class DeletePhysicalExaminationCommandHandler : IRequestHandler<DeletePhysicalExaminationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePhysicalExaminationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeletePhysicalExaminationCommand request, CancellationToken ct)
        {
            var entity = await _unitOfWork.PhysicalExaminations.GetByIdAsync(request.Id);
            if (entity == null) return false;

            await _unitOfWork.PhysicalExaminations.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }
}

