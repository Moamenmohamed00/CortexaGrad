using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cortexa.Domain.Entities.Core;

namespace Cortexa.Infrastructure.Persistence.Configurations
{
    public class AdmissionConfiguration : IEntityTypeConfiguration<Admission>
    {
        public void Configure(EntityTypeBuilder<Admission> builder)
        {
            // ── Primary Key ────────────────────────────────────────────
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).HasMaxLength(20);

            // Audit fields
            builder.Property(a => a.CreatedBy).HasMaxLength(200);
            builder.Property(a => a.LastModifiedBy).HasMaxLength(200);

            // ── Properties ─────────────────────────────────────────────
            builder.Property(a => a.InitialDiagnosis)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.DischargeSummary)
                .HasMaxLength(2000);

            builder.Property(a => a.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(a => a.DischargeDisposition)
                .HasConversion<string>()
                .HasMaxLength(30);

            // ── Relationships – Parent FKs ─────────────────────────────
            builder.HasOne(a => a.Patient)
                .WithMany(p => p.Admissions)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Doctor)
                .WithMany(d => d.Admissions)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Room)
                .WithMany()
                .HasForeignKey(a => a.RoomId)
                .OnDelete(DeleteBehavior.SetNull);

            // Bed ↔ Admission is configured in BedConfiguration (one-to-one)

            // ── Relationships – Clinical Collections ───────────────────
            builder.HasMany(a => a.VitalSigns)
                .WithOne(v => v.Admission)
                .HasForeignKey(v => v.AdmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.FluidBalances)
                .WithOne(f => f.Admission)
                .HasForeignKey(f => f.AdmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.NursingNotes)
                .WithOne(n => n.Admission)
                .HasForeignKey(n => n.AdmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.CaseHistories)
                .WithOne(c => c.Admission)
                .HasForeignKey(c => c.AdmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.PhysicalExaminations)
                .WithOne(pe => pe.Admission)
                .HasForeignKey(pe => pe.AdmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Medications)
                .WithOne(m => m.Admission)
                .HasForeignKey(m => m.AdmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.InterventionProcedures)
                .WithOne(ip => ip.Admission)
                .HasForeignKey(ip => ip.AdmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ── Relationships – Diagnostics Collections ────────────────
            builder.HasMany(a => a.LabOrders)
                .WithOne(lo => lo.Admission)
                .HasForeignKey(lo => lo.AdmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Imaging)
                .WithOne(i => i.Admission)
                .HasForeignKey(i => i.AdmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Cultures)
                .WithOne(c => c.Admission)
                .HasForeignKey(c => c.AdmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ── Relationships – AI Collections ─────────────────────────
            builder.HasMany(a => a.Alerts)
                .WithOne(al => al.Admission)
                .HasForeignKey(al => al.AdmissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
