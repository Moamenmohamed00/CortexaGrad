using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class ImagingRepository : GenericRepository<Imaging>, IImagingRepository
    {
        public ImagingRepository(CortexaDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Imaging>> GetImagingByAdmissionIdAsync(string admissionId)
        {
            return await _context.Imagings
                .Where(i => i.AdmissionId == admissionId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Imaging>> GetImagingByPatientIdAsync(string patientId)
        {
            return await _context.Imagings
                .Where(i => _context.Admissions.Any(a => a.PatientId == patientId && a.Id == i.AdmissionId))
                .ToListAsync();
        }

        public async Task AddImagingAsync(Imaging imaging)
        {
            await _context.Imagings.AddAsync(imaging);
        }
    }
}
