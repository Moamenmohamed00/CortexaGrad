using System.Collections.Generic;
using System.Threading.Tasks;
using Cortexa.Domain.Entities.Diagnostics;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface ILabRepository : IGenericRepository<LabOrder>
    {
        Task<IReadOnlyList<LabOrder>> GetPendingOrdersAsync();
        Task<IReadOnlyList<LabOrder>> GetOrdersByAdmissionIdAsync(string admissionId);
        Task<IReadOnlyList<LabResult>> GetResultsByOrderIdAsync(string orderId);
        Task AddResultAsync(LabResult result);
        Task AddLabOrderAsync(LabOrder order);
    }
}
