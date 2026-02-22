using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cortexa.Domain.Entities.Clinical;

namespace Cortexa.Infrastructure.Persistence.Configurations
{
    public class MedicationConfiguration : IEntityTypeConfiguration<Medications>
    {
        public void Configure(EntityTypeBuilder<Medications> builder)
        {
            // ── Primary Key ────────────────────────────────────────────
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).HasMaxLength(20);

            // Audit fields
            builder.Property(m => m.CreatedBy).HasMaxLength(200);
            builder.Property(m => m.LastModifiedBy).HasMaxLength(200);

            // ── Properties ─────────────────────────────────────────────
            builder.Property(m => m.DrugName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.DoseUnit)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(m => m.Route)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            // ── Relationships ──────────────────────────────────────────
            builder.HasOne(m => m.Admission)
                .WithMany(a => a.Medications)
                .HasForeignKey(m => m.AdmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Doctor)
                .WithMany(d => d.Medications)
                .HasForeignKey(m => m.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
