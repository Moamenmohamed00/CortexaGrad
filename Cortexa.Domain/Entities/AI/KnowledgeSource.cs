using Cortexa.Domain.Common;
using Cortexa.Domain.Entities.Actors;

namespace Cortexa.Domain.Entities.AI
{
    public class KnowledgeSource : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // PDF, Web, Text
        public string URL { get; set; } = string.Empty; // Or FilePath

        public string DoctorId { get; set; } = string.Empty; // Uploaded by
        public Doctor Doctor { get; set; } = null!; // EF Core will set this
    }
}
