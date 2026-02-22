using System.Threading.Tasks;
using Cortexa.Application.Dtos.AI;

namespace Cortexa.Application.Interfaces.Services
{
    public interface IAIService
    {
        Task<float> GenerateRiskScoreAsync(string patientId);
        Task<RagQueryDto> ProcessRagQueryAsync(RagQueryDto query);
        Task<AlertDto> GenerateAlertAsync(string admissionId, AlertDto alert);
    }
}
