using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Dtos.Diagnostics;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Services;
using Cortexa.Domain.Entities.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Infrastructure.Services
{
    public class DiagnosticService : IDiagnosticService
    { 
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;

        public DiagnosticService(IImageService imageService, IUnitOfWork unitOfWork)
        {
            _imageService = imageService;
            _unitOfWork = unitOfWork;
        }
        public Task<CultureDto> AddCultureResultAsync(string admissionId, CultureDto culture)
        {
            throw new NotImplementedException();
        }

        public Task<LabResultDto> AddLabResultAsync(string orderId, LabResultDto result)
        {
            throw new NotImplementedException();
        }

        public Task<LabOrderDto> OrderLabTestAsync(string admissionId, LabOrderDto order)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDto<bool>> UploadImagingResultAsync(UploadImagingDto uploadImagingDto)
        {
            if (uploadImagingDto.Content == null || uploadImagingDto.Content.Length == 0)
            {
                return new ResultDto<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "No image content provided."
                };
            }

            var admissionExists = await _unitOfWork.Admissions.GetByIdAsync(uploadImagingDto.AdmissionId);
            if (admissionExists == null)
            {
                return new ResultDto<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Admission not found."
                };
            }

            var extension = Path.GetExtension(uploadImagingDto.FileName);
            var filename = $"{uploadImagingDto.AdmissionId}_{uploadImagingDto.Type}_{DateTime.UtcNow.Ticks}{extension}";

            var uploadResult = await UploadPhotoAsync(uploadImagingDto.Content, filename);

            if (uploadResult == (null, null))
            {
                return new ResultDto<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Failed to upload image to the cloud service."
                };
            }

            var imagingFile = new ImagingFile
            {
                FileName = uploadImagingDto.FileName,
                Url = uploadResult.Item1,
                PublicId = uploadResult.Item2,
                Size = uploadImagingDto.Content.Length
            };

            var imaging = new Imaging
            {
                AdmissionId = uploadImagingDto.AdmissionId,
                Type = uploadImagingDto.Type,
                Findings = uploadImagingDto.Findings,
                Date = uploadImagingDto.Date,
                DoctorId = uploadImagingDto.DoctorId,
                Files = new List<ImagingFile> { imagingFile }
            };

            await _unitOfWork.Imaging.AddAsync(imaging);
            await _unitOfWork.SaveChangesAsync();

            return new ResultDto<bool>
            {
                Data = true,
                Success = true,
                Message = "Imaging result uploaded successfully."
            };
        }

        private async Task<(string,string)> UploadPhotoAsync(byte[] photo, string fileName)
        {
            using var ms = new MemoryStream(photo);
            var result = await _imageService.UploadImageAsync(ms, fileName);

            if (string.IsNullOrEmpty(result.Url) || string.IsNullOrEmpty(result.PublicId))
            {
                return (null, null);
            }
            
            return (result.Url, result.PublicId);
        }
    }
}
