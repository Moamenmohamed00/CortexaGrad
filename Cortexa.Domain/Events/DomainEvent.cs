using System;

namespace Cortexa.Domain.Events
{
    /// <summary>
    /// Base class for all domain events
    /// </summary>
    public abstract class DomainEvent
    {
        public string EventId { get; }
        public DateTime OccurredOn { get; }

        protected DomainEvent()
        {
            EventId = Guid.NewGuid().ToString();
            OccurredOn = DateTime.UtcNow;
        }
    }
}

