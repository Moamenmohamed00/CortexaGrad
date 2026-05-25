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
            if (uploadImagingDto.Files == null || uploadImagingDto.Files.Count == 0)
            {
                return new ResultDto<bool> { Data = false, Success = false, Message = "No files provided." };
            }

            var admissionExists = await _unitOfWork.Admissions.GetByIdAsync(uploadImagingDto.AdmissionId);
            if (admissionExists == null)
            {
                return new ResultDto<bool> { Data = false, Success = false, Message = "Admission not found." };
            }

            var doctorExists = await _unitOfWork.Doctors.GetByIdAsync(uploadImagingDto.DoctorId);
            if (doctorExists == null)
            {
                return new ResultDto<bool> { Data = false, Success = false, Message = "Doctor not found." };
            }

            var imaging = new Imaging
            {
                AdmissionId = uploadImagingDto.AdmissionId,
                Type = uploadImagingDto.Type,
                Findings = uploadImagingDto.Findings,
                Date = uploadImagingDto.Date,
                DoctorId = uploadImagingDto.DoctorId,
                Files = new List<ImagingFile>()
            };

            foreach (var fileDto in uploadImagingDto.Files)
            {
                var extension = Path.GetExtension(fileDto.FileName);

                // Expert Tip: Use Guid instead of Ticks inside fast loops to avoid filename collisions
                var filename = $"{uploadImagingDto.AdmissionId}_{uploadImagingDto.Type}_{Guid.NewGuid()}{extension}";

                var uploadResult = await UploadPhotoAsync(fileDto.Content, filename);

                if (uploadResult != (null, null))
                {
                    imaging.Files.Add(new ImagingFile
                    {
                        FileName = fileDto.FileName,
                        Url = uploadResult.Item1,
                        PublicId = uploadResult.Item2,
                        Size = fileDto.Content.Length
                    });
                }
            }

            if (imaging.Files.Count == 0)
            {
                return new ResultDto<bool> { Data = false, Success = false, Message = "Failed to upload any image to the cloud service." };
            }

            await _unitOfWork.Imaging.AddAsync(imaging);
            await _unitOfWork.SaveChangesAsync();

            return new ResultDto<bool> { Data = true, Success = true, Message = $"{imaging.Files.Count} imaging files uploaded successfully." };
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
