using Cortexa.Application.Dtos.Patient;
using Cortexa.Domain.Enums;
using MediatR;

namespace Cortexa.Application.Features.Patients.Commands
{
    public record AdmitPatientCommand(
        string Name,
        string NationalId,
        DateTime DateOfBirth,
        Gender Gender,
        string? Email,
        string? Phone,
        string Street,
        string City,
        string State,
        string? ZipCode,
        string? Country,
        BloodType BloodType,
        string? DiagnosisSummary,
        string DoctorId,
        string InitialDiagnosis,
        string? BedId,
        string? RoomId
    ) : IRequest<PatientAdmissionDto>;

}
