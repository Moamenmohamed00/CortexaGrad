using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cortexa.Domain.Entities.Actors;

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

            builder.Property(d => d.PhoneNumbers)
                .HasColumnType("nvarchar(max)")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null)!
                );

            builder.OwnsOne(d => d.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(300).HasColumnName("Address_Street");
                address.Property(a => a.City).HasMaxLength(100).HasColumnName("Address_City");
                address.Property(a => a.State).HasMaxLength(100).HasColumnName("Address_State");
                address.Property(a => a.ZipCode).HasMaxLength(20).HasColumnName("Address_ZipCode");
            });

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
