using MediatR;
using Cortexa.Domain.Entities.Diagnostics;
using Cortexa.Domain.Enums;
using Cortexa.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Diagnostics.Commands
{
    public class UploadImagingCommand : IRequest<string>
    {
        public string AdmissionId { get; set; } = string.Empty;
        public ImagingType Type { get; set; }
        public string? Findings { get; set; }
        public DateTime Date { get; set; }
        public string DoctorId { get; set; } = string.Empty;
    }

    public class UploadImagingCommandHandler : IRequestHandler<UploadImagingCommand, string>
    {
        private readonly IImagingRepository _imagingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UploadImagingCommandHandler(IImagingRepository imagingRepository, IUnitOfWork unitOfWork)
        {
            _imagingRepository = imagingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(UploadImagingCommand request, CancellationToken cancellationToken)
        {
            var entity = new Imaging
            {
                AdmissionId = request.AdmissionId,
                Type = request.Type,
                Findings = request.Findings,
                Date = request.Date,
                DoctorId = request.DoctorId
            };

            await _imagingRepository.AddImagingAsync(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
