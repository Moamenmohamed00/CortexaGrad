using System.Threading.Tasks;
using Cortexa.Application.Dtos.Core;
using Cortexa.Application.Dtos.Diagnostics;

namespace Cortexa.Application.Interfaces.Services
{
    public interface IDiagnosticService
    {
        Task<LabOrderDto> OrderLabTestAsync(string admissionId, LabOrderDto order);
        Task<LabResultDto> AddLabResultAsync(string orderId, LabResultDto result);
        Task<ResultDto<bool>> UploadImagingResultAsync(UploadImagingDto uploadImagingDto);
        Task<CultureDto> AddCultureResultAsync(string admissionId, CultureDto culture);
    }
}
