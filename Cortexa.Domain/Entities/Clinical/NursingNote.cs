using Cortexa.Domain.Common;

namespace Cortexa.Domain.Entities.Clinical
{
    public class NursingNote : BaseEntity
    {
        public Guid AdmissionId { get; private set; }
        public Guid NurseId { get; private set; }
        public string NoteText { get; private set; }
        public DateTime NoteDateTime { get; private set; }
    
    }
}
