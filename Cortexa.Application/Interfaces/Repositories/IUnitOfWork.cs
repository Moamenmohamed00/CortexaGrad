using Cortexa.Application.Interfaces.Repositories;

namespace Cortexa.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        // Repository accessors
        IPatientRepository Patients { get; }
        IAdmissionRepository Admissions { get; }
        IClinicalRepository Clinical { get; }
        ILabRepository Labs { get; }
        IImagingRepository Imaging { get; }
        IDoctorRepository Doctors { get; }
        IAIRepository AI { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
