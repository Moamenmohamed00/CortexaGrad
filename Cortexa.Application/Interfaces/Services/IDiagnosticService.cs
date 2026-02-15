using System.Threading.Tasks;
using Cortexa.Application.Dtos.Diagnostics;

namespace Cortexa.Application.Interfaces.Services
{
    public interface IDiagnosticService
    {
        Task<LabOrderDto> OrderLabTestAsync(string admissionId, LabOrderDto order);
        Task<LabResultDto> AddLabResultAsync(string orderId, LabResultDto result);
        Task<ImagingDto> UploadImagingResultAsync(string admissionId, ImagingDto imaging);
        Task<CultureDto> AddCultureResultAsync(string admissionId, CultureDto culture);
    }
}
