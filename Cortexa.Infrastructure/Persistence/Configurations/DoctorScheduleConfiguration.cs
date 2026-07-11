using Cortexa.Domain.Entities.StaffSchedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cortexa.Infrastructure.Persistence.Configurations
{
    public class DoctorScheduleConfiguration : IEntityTypeConfiguration<DoctorSchedule>
    {
        public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
        {
            // ── Primary Key & Base Fields ──────────────────────────────
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).HasMaxLength(20);

            builder.Property(d => d.CreatedBy).HasMaxLength(200);
            builder.Property(d => d.LastModifiedBy).HasMaxLength(200);


            builder.Property(s => s.ShiftStart)
                .IsRequired();

            builder.Property(s => s.ShiftEnd)
                .IsRequired();

            builder.Property(s => s.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(s => s.IsOnCall)
                .HasDefaultValue(false);

            // Indexing for high-performance real-time queries in ICU
            builder.HasIndex(s => new { s.ShiftStart, s.ShiftEnd, s.Status });
        }
    }
}
