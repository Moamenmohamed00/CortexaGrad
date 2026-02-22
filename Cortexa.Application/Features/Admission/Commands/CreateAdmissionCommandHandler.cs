using Cortexa.Application.Common.Interfaces;
using Cortexa.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;



namespace Cortexa.Application.Features.Admission.Commands
{
    public record CreateAdmissionCommand(
        string PatientId,
        string DoctorId,
        DateTime AdmissionDate,
        string InitialDiagnosis,
        string? RoomId,
        string? BedId
    ) : IRequest<string>; // returns AdmissionId
    public class CreateAdmissionCommandHandler
        : IRequestHandler<CreateAdmissionCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public CreateAdmissionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(
            CreateAdmissionCommand request,
            CancellationToken cancellationToken)
        {
            // Validate Patient exists
            var patientExists = await _context.Patients
                .AnyAsync(p => p.Id == request.PatientId, cancellationToken);

            if (!patientExists)
                throw new Exception("Patient not found.");

            // Validate Doctor exists
            var doctorExists = await _context.Doctors
                .AnyAsync(d => d.Id == request.DoctorId, cancellationToken);

            if (!doctorExists)
                throw new Exception("Doctor not found.");

            var admission = new Cortexa.Domain.Entities.Core.Admission
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                AdmissionDate = request.AdmissionDate,
                InitialDiagnosis = request.InitialDiagnosis,
                RoomId = request.RoomId,
                BedId = request.BedId,
                Status = AdmissionStatus.Active
            };

            _context.Admissions.Add(admission);

            await _context.SaveChangesAsync(cancellationToken);

            return admission.Id;
        }
    }
}
