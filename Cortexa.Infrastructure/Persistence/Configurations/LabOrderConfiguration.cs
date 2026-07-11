using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cortexa.Domain.Entities.Diagnostics;

namespace Cortexa.Infrastructure.Persistence.Configurations
{
    public class LabOrderConfiguration : IEntityTypeConfiguration<LabOrder>
    {
        public void Configure(EntityTypeBuilder<LabOrder> builder)
        {
            // ── Primary Key ────────────────────────────────────────────
            builder.HasKey(lo => lo.Id);
            builder.Property(lo => lo.Id).HasMaxLength(20);

            // Audit fields
            builder.Property(lo => lo.CreatedBy).HasMaxLength(200);
            builder.Property(lo => lo.LastModifiedBy).HasMaxLength(200);

            // ── Properties ─────────────────────────────────────────────
            builder.Property(lo => lo.TestName)
                .IsRequired()
                .HasMaxLength(200);

            // ── Relationships ──────────────────────────────────────────
            builder.HasOne(lo => lo.Admission)
                .WithMany(a => a.LabOrders)
                .HasForeignKey(lo => lo.AdmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(lo => lo.Doctor)
                .WithMany(d => d.LabOrders)
                .HasForeignKey(lo => lo.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(lo => lo.Results)
                .WithOne(r => r.LabOrder)
                .HasForeignKey(r => r.LabOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
