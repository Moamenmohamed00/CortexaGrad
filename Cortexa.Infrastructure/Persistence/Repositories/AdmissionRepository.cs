using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class AdmissionRepository : GenericRepository<Admission>, IAdmissionRepository
    {
        public AdmissionRepository(CortexaDbContext context) : base(context) { }

        public override async Task<Admission?> GetByIdAsync(string id)
        {
            return await _context.Admissions
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Bed)
                .Include(a => a.VitalSigns)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

        }
        public async Task<IReadOnlyList<Admission>> GetActiveAdmissionsAsync(CancellationToken cancellationToken)
        {
            return await _context.Admissions
                .Where(a => a.Status == AdmissionStatus.Active)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Bed)
                .Include(a=>a.VitalSigns)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Admission>> GetActiveAdmissionsByPatientIdAsync(string patientId, CancellationToken cancellationToken)
        {
            return await _context.Admissions
                .Where(a => a.PatientId == patientId && a.Status == AdmissionStatus.Active)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Bed)
                .Include(a => a.VitalSigns)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        }

        public async Task<IReadOnlyList<Admission>> GetAdmissionsByPatientIdAsync(string patientId, CancellationToken cancellationToken)
        {
            return await _context.Admissions
                .Where(a => a.PatientId == patientId)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Bed)
                .Include(a => a.VitalSigns)
                .OrderByDescending(a => a.AdmissionDate)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Admission?> GetByIdWithPatientDataAsync(string admissionId, CancellationToken cancellationToken, bool includeDetails = false)
        {
            IQueryable<Admission> query = _context.Admissions;

            if (includeDetails)
            {
                query = query
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Include(a => a.Bed);
            }

            return await query
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == admissionId, cancellationToken);
        }

        public async Task<IReadOnlyList<Admission>> GetAdmissionsByDateAsync(DateTime date, CancellationToken cancellationToken, bool includeDetails = false)
        {
            IQueryable<Admission> query = _context.Admissions;

            if (includeDetails)
            {
                query = query
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Include(a => a.Bed);
            }

            return await query
                .Where(a => a.AdmissionDate >= date.Date &&
                            a.AdmissionDate < date.Date.AddDays(1))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }


        public async Task<IReadOnlyList<Admission>> GetDischargesByDateAsync(DateTime date, CancellationToken cancellationToken, bool includeDetails = false)
        {
            IQueryable<Admission> query = _context.Admissions;
            if (includeDetails)
            {
                query = query
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Include(a => a.Bed);
            }
            return await query
                .Where(a => a.DischargeDate.HasValue &&
                            a.DischargeDate.Value >= date.Date &&
                            a.DischargeDate.Value < date.Date.AddDays(1))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}