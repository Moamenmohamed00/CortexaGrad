using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Dtos.Diagnostics;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Services;
using Cortexa.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cortexa.Application.Features.Diagnostics.Commands
{
    public class UploadImagingCommand : IRequest<ResultDto<bool>>
    {
        public List<UploadImagingFileCommand> Files { get; set; } = new();
        public string AdmissionId { get; set; } = string.Empty;
        public ImagingType Type { get; set; }
        public string? Findings { get; set; }
        public DateTime Date { get; set; }
        public string DoctorId { get; set; } = string.Empty;
    }

    public class UploadImagingFileCommand
    {
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = string.Empty;
    }

    public class UploadImagingCommandHandler : IRequestHandler<UploadImagingCommand, ResultDto<bool>>
    {
        private readonly IDiagnosticService _diagnosticService;

        public UploadImagingCommandHandler(IDiagnosticService diagnosticService)
        {
            _diagnosticService = diagnosticService;
        }

        public async Task<ResultDto<bool>> Handle(UploadImagingCommand request, CancellationToken cancellationToken)
        {
            if (request.Files == null || !request.Files.Any())
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "No files provided to upload."
                };
            }

            var files = request.Files.Select(f => new UploadImagingFileDto
            {
                Content = f.Content,
                FileName = f.FileName
            }).ToList();

            var result = await _diagnosticService.UploadImagingResultAsync(new UploadImagingDto
            {
                Files = files,
                AdmissionId = request.AdmissionId,
                Type = request.Type,
                Findings = request.Findings,
                Date = request.Date,
                DoctorId = request.DoctorId
            });

            if (result.Success)
            {
                return new ResultDto<bool> { Success = true, Data = true, Message = result.Message ?? "Imaging uploaded successfully." };
            }
            else
            {
                return new ResultDto<bool> { Success = false, Data = false, Message = result.Message ?? "Failed to upload imaging." };
            }
        }
    }
}
