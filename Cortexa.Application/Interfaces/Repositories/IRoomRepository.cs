using Cortexa.Domain.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<IEnumerable<Room>> GetRoomsWithBedsAvailableAsync();
    }
}
