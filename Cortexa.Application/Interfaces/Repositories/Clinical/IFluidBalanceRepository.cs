using Cortexa.Domain.Entities.Clinical;

namespace Cortexa.Application.Interfaces.Repositories.Clinical
{
    public interface IFluidBalanceRepository : IGenericRepository<FluidBalance>
    {
        Task<IReadOnlyList<FluidBalance>> GetByAdmissionIdAsync(string admissionId);
    }
}
