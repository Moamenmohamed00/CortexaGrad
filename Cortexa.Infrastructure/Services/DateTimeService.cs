using Cortexa.Application.Common.Interfaces;

namespace Cortexa.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
