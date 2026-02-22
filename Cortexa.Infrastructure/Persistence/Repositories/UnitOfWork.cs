using Cortexa.Application.Interfaces.Repositories;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CortexaDbContext _context;
        private bool _disposed;

        // Lazy-loaded backing fields
        private IPatientRepository? _patients;
        private IAdmissionRepository? _admissions;
        private IClinicalRepository? _clinical;
        private ILabRepository? _labs;
        private IImagingRepository? _imaging;
        private IDoctorRepository? _doctors;
        private IAIRepository? _ai;

        public UnitOfWork(CortexaDbContext context)
        {
            _context = context;
        }

        // ── Repository Accessors (lazy-loaded) ─────────────────────────
        public IPatientRepository Patients =>
            _patients ??= new PatientRepository(_context);

        public IAdmissionRepository Admissions =>
            _admissions ??= new AdmissionRepository(_context);

        public IClinicalRepository Clinical =>
            _clinical ??= new ClinicalRepository(_context);

        public ILabRepository Labs =>
            _labs ??= new LabRepository(_context);

        public IImagingRepository Imaging =>
            _imaging ??= new ImagingRepository(_context);

        public IDoctorRepository Doctors =>
            _doctors ??= new DoctorRepository(_context);

        public IAIRepository AI =>
            _ai ??= new AIRepository(_context);

        // ── Persistence ────────────────────────────────────────────────
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        // ── Disposal ───────────────────────────────────────────────────
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}
