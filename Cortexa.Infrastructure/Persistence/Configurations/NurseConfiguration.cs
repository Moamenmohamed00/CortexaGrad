using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cortexa.Domain.Entities.Actors;

namespace Cortexa.Infrastructure.Persistence.Configurations
{
    public class NurseConfiguration : IEntityTypeConfiguration<Nurse>
    {
        public void Configure(EntityTypeBuilder<Nurse> builder)
        {
            // ── Primary Key & Base Fields ──────────────────────────────
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Id).HasMaxLength(20);

            builder.Property(n => n.CreatedBy).HasMaxLength(200);
            builder.Property(n => n.LastModifiedBy).HasMaxLength(200);

            // ── AppUser Fields ─────────────────────────────────────────
            builder.Property(n => n.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(n => n.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(n => n.PhoneNumbers)
                .HasColumnType("nvarchar(max)")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null)!
                );

            builder.OwnsOne(n => n.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(300).HasColumnName("Address_Street");
                address.Property(a => a.City).HasMaxLength(100).HasColumnName("Address_City");
                address.Property(a => a.State).HasMaxLength(100).HasColumnName("Address_State");
                address.Property(a => a.ZipCode).HasMaxLength(20).HasColumnName("Address_ZipCode");
            });

            // ── Nurse-Specific Fields ──────────────────────────────────
            builder.Property(n => n.Department)
                .HasMaxLength(200);

        }
    }
}
