using Cortexa.Domain.Entities.Actors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cortexa.Infrastructure.Persistence.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            // ── Primary Key & Base Fields ──────────────────────────────
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).HasMaxLength(20);

            builder.Property(d => d.CreatedBy).HasMaxLength(200);
            builder.Property(d => d.LastModifiedBy).HasMaxLength(200);

            // ── AppUser Fields ─────────────────────────────────────────
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.PhoneNumber)
                .HasColumnType("nvarchar(max)")
             ;

            // Store Enum as string in the database for better readability
            builder.Property(d => d.AvailabilityStatus)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            // Relationship: One Doctor has many Schedules
            builder.HasMany(d => d.Schedules)
                .WithOne(s => s.Doctor)
                .HasForeignKey(s => s.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(d => d.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(300).HasColumnName("Address_Street");
                address.Property(a => a.City).HasMaxLength(100).HasColumnName("Address_City");
                address.Property(a => a.State).HasMaxLength(100).HasColumnName("Address_State");
                address.Property(a => a.ZipCode).HasMaxLength(20).HasColumnName("Address_ZipCode");
            });
            builder.Property(p => p.NationalId)
    .IsRequired()
    .HasMaxLength(20);
            // ── Doctor-Specific Fields ─────────────────────────────────
            builder.Property(d => d.Specialty)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.Department)
                .HasMaxLength(200);

            // ── Relationships ──────────────────────────────────────────
            builder.HasMany(d => d.Admissions)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
