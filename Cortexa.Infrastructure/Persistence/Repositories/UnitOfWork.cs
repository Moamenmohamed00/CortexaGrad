using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Application.Interfaces.Repositories.Clinical;
using Cortexa.Infrastructure.Persistence.Repositories.Clinical;

namespace Cortexa.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CortexaDbContext _context;
        private bool _disposed;

        // Lazy-loaded backing fields
        private IPatientRepository? _patients;
        private IAdmissionRepository? _admissions;
        //private IClinicalRepository? _clinical;
        private IVitalSignsRepository? _vitalSigns;
        private IPhysicalExaminationRepository? _physicalExamination;
        private ICaseHistoryRepository? _caseHistory;
        private IMedicationRepository? _medication;
        private INursingNotesRepository? _nursingNotes;
        private IFluidBalanceRepository? _fluidBalance;
        private IInterventionProcedureRepository? _interventionProcedure;
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

        //public IClinicalRepository Clinical =>
        //    _clinical ??= new ClinicalRepository(_context);
        public IVitalSignsRepository VitalSigns =>
            _vitalSigns ??= new VitalSignsRepository(_context);
        public IPhysicalExaminationRepository PhysicalExaminations =>
            _physicalExamination ??= new PhysicalExaminationRepository(_context);
        public ICaseHistoryRepository CaseHistories => 
            _caseHistory ??= new CaseHistoryRepository(_context);

        public IMedicationRepository Medications => 
            _medication ??= new MedicationsRepository(_context);
        public INursingNotesRepository NursingNotes => 
            _nursingNotes ??= new NursingNotesRepository(_context);
        public IFluidBalanceRepository FluidBalances => 
            _fluidBalance ??= new FluidBalanceRepository(_context);
        public IInterventionProcedureRepository InterventionProcedures => 
            _interventionProcedure ??= new InterventionProcedureRepository(_context);

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
