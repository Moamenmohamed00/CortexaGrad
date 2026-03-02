using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Infrastructure;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class BedRepository : GenericRepository<Bed>, IBedRepository
    {
        public BedRepository(CortexaDbContext context) : base(context) { }
    }
}
