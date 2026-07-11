using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cortexa.Domain.Common;

namespace Cortexa.Infrastructure.Persistence.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

            builder.Property(a => a.EntityId)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(a => a.EntityName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Type)
                .IsRequired();

            builder.Property(a => a.UserId)
                .HasMaxLength(200);

            builder.Property(a => a.AffectedColumns)
                .HasMaxLength(1000);

            builder.HasIndex(a => a.EntityId);
            builder.HasIndex(a => a.EntityName);
            builder.HasIndex(a => a.Timestamp);
        }
    }
}
