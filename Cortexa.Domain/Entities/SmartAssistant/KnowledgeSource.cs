using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Identity;

namespace Cortexa.Domain.Entities.SmartAssistant
{
    public class KnowledgeSource : BaseEntity
    {
        public Guid DoctorId { get; private set; }
        public Doctor Doctor { get; private set; }


        public string Title { get; private set; }
        public string Type { get; private set; }
        public string URL { get; private set; }

    }
}
