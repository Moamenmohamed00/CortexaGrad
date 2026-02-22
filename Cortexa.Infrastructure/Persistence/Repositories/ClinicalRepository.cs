using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Clinical;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class ClinicalRepository : GenericRepository<VitalSigns>, IClinicalRepository
    {
        public ClinicalRepository(CortexaDbContext context) : base(context) { }

        public async Task<IReadOnlyList<VitalSigns>> GetVitalSignsByAdmissionIdAsync(string admissionId)
        {
            return await _context.VitalSigns
                .Where(v => v.AdmissionId == admissionId)
                .OrderByDescending(v => v.RecordedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Medications>> GetMedicationsByAdmissionIdAsync(string admissionId)
        {
            return await _context.Medications
                .Where(m => m.AdmissionId == admissionId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<NursingNotes>> GetNursingNotesByAdmissionIdAsync(string admissionId)
        {
            return await _context.NursingNotes
                .Where(n => n.AdmissionId == admissionId)
                .OrderByDescending(n => n.NoteDateTime)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<FluidBalance>> GetFluidBalanceByAdmissionIdAsync(string admissionId)
        {
            return await _context.FluidBalances
                .Where(f => f.AdmissionId == admissionId)
                .OrderByDescending(f => f.RecordedAt)
                .ToListAsync();
        }

        public async Task AddVitalSignsAsync(VitalSigns vitalSigns, CancellationToken cancellationToken)
        {
            await _context.VitalSigns.AddAsync(vitalSigns, cancellationToken);
        }

        public async Task AddMedicationAsync(Medications medication, CancellationToken cancellationToken)
        {
            await _context.Medications.AddAsync(medication, cancellationToken);
        }

        public async Task AddNursingNoteAsync(NursingNotes nursingNote, CancellationToken cancellationToken)
        {
            await _context.NursingNotes.AddAsync(nursingNote, cancellationToken);
        }

        public async Task AddFluidBalanceAsync(FluidBalance fluidBalance, CancellationToken cancellationToken)
        {
            await _context.FluidBalances.AddAsync(fluidBalance, cancellationToken);
        }
    }
}
