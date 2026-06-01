using Cortexa.Domain.Entities.StaffSchedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cortexa.Infrastructure.Persistence.Configurations
{
    public class NurseScheduleConfiguration : IEntityTypeConfiguration<NurseSchedule>
    {
        public void Configure(EntityTypeBuilder<NurseSchedule> builder)
        {
            // ── Primary Key & Base Fields ──────────────────────────────
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Id).HasMaxLength(20);

            builder.Property(n => n.CreatedBy).HasMaxLength(200);
            builder.Property(n => n.LastModifiedBy).HasMaxLength(200);

            builder.Property(s => s.ShiftStart)
                .IsRequired();

            builder.Property(s => s.ShiftEnd)
                .IsRequired();

            builder.Property(s => s.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            // Indexing for faster lookups when assigning critical tasks
            builder.HasIndex(s => new { s.ShiftStart, s.ShiftEnd, s.Status });
        }
    }
}
