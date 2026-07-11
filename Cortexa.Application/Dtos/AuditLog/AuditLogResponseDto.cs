using Cortexa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Dtos.AuditLog
{
    public class AuditLogResponseDto
    {
        public long Id { get; set; }
        public string EntityId { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public AuditType Type { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string? AffectedColumns { get; set; }
        public DateTime Timestamp { get; set; }
        public string? UserId { get; set; }
    }
}
