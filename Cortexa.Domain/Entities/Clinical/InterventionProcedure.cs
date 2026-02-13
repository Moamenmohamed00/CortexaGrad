using Cortexa.Domain.Common;

namespace Cortexa.Domain.Entities.Clinical
{
    public class InterventionProcedure : BaseEntity
    {
        public Guid AdmissionId { get; private set; }
        public Guid NurseId { get; private set; }

        public string Type { get; private set; }
        public int Size { get; private set; }

        public DateTime InsertionDate { get; private set; }
        public DateTime? RemovalDate { get; private set; }
    }
}