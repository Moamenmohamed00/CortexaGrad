using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cortexa.Domain.Entities.AI;

namespace Cortexa.Infrastructure.Persistence.Configurations
{
    public class AlertConfiguration : IEntityTypeConfiguration<Alert>
    {
        public void Configure(EntityTypeBuilder<Alert> builder)
        {
            // ── Primary Key ────────────────────────────────────────────
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).HasMaxLength(20);

            // Audit fields
            builder.Property(a => a.CreatedBy).HasMaxLength(200);
            builder.Property(a => a.LastModifiedBy).HasMaxLength(200);

            // ── Properties ─────────────────────────────────────────────
            builder.Property(a => a.AlertMessage)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.Severity)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(a => a.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            // ── Relationships ──────────────────────────────────────────
            builder.HasOne(a => a.Admission)
                .WithMany(adm => adm.Alerts)
                .HasForeignKey(a => a.AdmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.AlertOverrideLogs)
                .WithOne(aol => aol.Alert)
                .HasForeignKey(aol => aol.AlertId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
