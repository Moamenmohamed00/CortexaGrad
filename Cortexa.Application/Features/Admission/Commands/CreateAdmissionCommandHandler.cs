using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Infrastructure;
using Cortexa.Domain.Enums;
using Cortexa.Domain.Exceptions;
using Cortexa.Domain.Services;

using MediatR;

namespace Cortexa.Application.Features.Admission.Commands
{
    public record CreateAdmissionCommand(
        string PatientId,
        string DoctorId,
        DateTime AdmissionDate,
        string InitialDiagnosis,
        string? RoomId,
        string? BedId
    ) : IRequest<string>;

    public class CreateAdmissionCommandHandler : IRequestHandler<CreateAdmissionCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateAdmissionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateAdmissionCommand request, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(request.PatientId);
            if (patient == null) throw new Exception("Patient not found.");

            var activeAdmission = await _unitOfWork.Admissions.GetActiveAdmissionsByPatientIdAsync(patient.Id);
            if (activeAdmission != null)
            {
                throw new PatientAlreadyAdmittedException(patient.Id, patient.Name);
            }

            var doctorExists = await _unitOfWork.Doctors.GetByIdAsync(request.DoctorId) != null;
            if (!doctorExists) throw new Exception("Doctor not found.");

            Bed? bed = null;
            if (!string.IsNullOrEmpty(request.BedId))
            {
                bed = await _unitOfWork.Beds.GetByIdAsync(request.BedId)
                    ?? throw new KeyNotFoundException($"Bed {request.BedId} not found.");

                if (bed.Status != BedStatus.Available)
                    throw new Exception("Bed is already occupied.");
            }

            
            
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

            await _unitOfWork.Admissions.AddAsync(admission, cancellationToken);

            if (bed != null)
            {
                bed.Status = BedStatus.Occupied;
                bed.CurrentAdmissionId = admission.Id;
                await _unitOfWork.Beds.UpdateAsync(bed);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return admission.Id;
        }
    }
}