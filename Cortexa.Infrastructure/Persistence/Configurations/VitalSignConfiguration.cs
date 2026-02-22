using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cortexa.Domain.Entities.Clinical;

namespace Cortexa.Infrastructure.Persistence.Configurations
{
    public class VitalSignConfiguration : IEntityTypeConfiguration<VitalSigns>
    {
        public void Configure(EntityTypeBuilder<VitalSigns> builder)
        {
            // ── Primary Key ────────────────────────────────────────────
            builder.HasKey(v => v.Id);
            builder.Property(v => v.Id).HasMaxLength(20);

            // Audit fields
            builder.Property(v => v.CreatedBy).HasMaxLength(200);
            builder.Property(v => v.LastModifiedBy).HasMaxLength(200);

            // ── Properties ─────────────────────────────────────────────
            builder.Property(v => v.InsulinGiven)
                .HasMaxLength(50);

            // Ignore computed property
            builder.Ignore(v => v.GCS_Total);

            // ── Relationships ──────────────────────────────────────────
            builder.HasOne(v => v.Admission)
                .WithMany(a => a.VitalSigns)
                .HasForeignKey(v => v.AdmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.Nurse)
                .WithMany(n => n.RecordedVitalSigns)
                .HasForeignKey(v => v.NurseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(v => v.Doctor)
                .WithMany(d => d.VerifiedVitalSigns)
                .HasForeignKey(v => v.DoctorId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
