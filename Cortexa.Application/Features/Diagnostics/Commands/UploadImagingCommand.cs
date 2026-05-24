using MediatR;
using Cortexa.Domain.Entities.Diagnostics;
using Cortexa.Domain.Enums;
using Cortexa.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Cortexa.Application.Interfaces.Services;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Dtos.Diagnostics;

namespace Cortexa.Application.Features.Diagnostics.Commands
{
    public class UploadImagingCommand : IRequest<ResultDto<bool>>
    {
        public byte[] Content { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string AdmissionId { get; set; } = string.Empty;
        public ImagingType Type { get; set; }
        public string? Findings { get; set; }
        public DateTime Date { get; set; }
        public string DoctorId { get; set; } = string.Empty;


    }

    public class UploadImagingCommandHandler : IRequestHandler<UploadImagingCommand, ResultDto<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDiagnosticService _diagnosticService;

        public UploadImagingCommandHandler(IUnitOfWork unitOfWork, IDiagnosticService diagnosticService)
        {
            _unitOfWork = unitOfWork;
            _diagnosticService = diagnosticService;
        }

        public async Task<ResultDto<bool>> Handle(UploadImagingCommand request, CancellationToken cancellationToken)
        {

            var result = await _diagnosticService.UploadImagingResultAsync(new UploadImagingDto
            {
                Content = request.Content,
                FileName = request.FileName,
                AdmissionId = request.AdmissionId,
                Type = request.Type,
                Findings = request.Findings,
                Date = request.Date,
                DoctorId = request.DoctorId
            });
            if (result.Success)
            {
                return new ResultDto<bool> { Success = true, Data = true , Message = "Imaging uploaded successfully." };
            }
            else
            {
                return new ResultDto<bool> { Success = false, Data = false, Message = "Failed to upload imaging." };
            }
        }
    }
}