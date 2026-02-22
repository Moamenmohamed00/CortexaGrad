using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cortexa.Domain.Entities.Infrastructure;

namespace Cortexa.Infrastructure.Persistence.Configurations
{
    public class BedConfiguration : IEntityTypeConfiguration<Bed>
    {
        public void Configure(EntityTypeBuilder<Bed> builder)
        {
            // ── Primary Key ────────────────────────────────────────────
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).HasMaxLength(20);

            // Audit fields
            builder.Property(b => b.CreatedBy).HasMaxLength(200);
            builder.Property(b => b.LastModifiedBy).HasMaxLength(200);

            // ── Properties ─────────────────────────────────────────────
            builder.Property(b => b.BedNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            // ── Relationships ──────────────────────────────────────────
            builder.HasOne(b => b.Room)
                .WithMany(r => r.Beds)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.CurrentAdmission)
                .WithOne(a => a.Bed)
                .HasForeignKey<Bed>(b => b.CurrentAdmissionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
