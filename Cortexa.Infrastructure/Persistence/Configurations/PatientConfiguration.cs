using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cortexa.Domain.Entities.Actors;

namespace Cortexa.Infrastructure.Persistence.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            // ── Primary Key & Base Fields ──────────────────────────────
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasMaxLength(20);

            // Audit fields (from BaseEntity)
            builder.Property(p => p.CreatedBy).HasMaxLength(200);
            builder.Property(p => p.LastModifiedBy).HasMaxLength(200);

            // ── AppUser Fields ─────────────────────────────────────────
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(200);

            // PhoneNumbers stored as JSON column
            builder.Property(p => p.PhoneNumbers)
                .HasColumnType("nvarchar(max)")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null)!
                );

            // Address as owned entity (maps to columns in same table)
            builder.OwnsOne(p => p.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(300).HasColumnName("Address_Street");
                address.Property(a => a.City).HasMaxLength(100).HasColumnName("Address_City");
                address.Property(a => a.State).HasMaxLength(100).HasColumnName("Address_State");
                address.Property(a => a.ZipCode).HasMaxLength(20).HasColumnName("Address_ZipCode");
            });

            // ── Patient-Specific Fields ────────────────────────────────
            builder.Property(p => p.FileNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.DiagnosisSummary)
                .HasMaxLength(1000);

            // Ignore computed properties
            builder.Ignore(p => p.Age);
            builder.Ignore(p => p.Sex);

            // ── Relationships ──────────────────────────────────────────
            builder.HasMany(p => p.Admissions)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.RAGQueries)
                .WithOne(r => r.Patient)
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
