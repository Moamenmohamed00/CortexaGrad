using Cortexa.Domain.Entities.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IRagRepository : IGenericRepository<RAGQuery>
    {
        Task<(IEnumerable<RAGQuery> Items, int TotalCount)> GetPaginatedByPatientIdAsync(string patientId, int pageNumber, int pageSize, CancellationToken ct = default);
    }
}
