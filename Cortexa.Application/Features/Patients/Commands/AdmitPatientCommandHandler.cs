using AutoMapper;
using Cortexa.Application.Dtos.Patient;
using Cortexa.Application.Interfaces.Repositories;
using Cortexa.Domain.Entities.Actors;
using Cortexa.Domain.Entities.Core;
using Cortexa.Domain.Entities.Infrastructure;
using Cortexa.Domain.Enums;
using Cortexa.Domain.Exceptions;
using Cortexa.Domain.ValueObjects;
using MediatR;
using AdmissionEntity = Cortexa.Domain.Entities.Core.Admission;

namespace Cortexa.Application.Features.Patients.Commands;

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

public class AdmitPatientCommandHandler : IRequestHandler<AdmitPatientCommand, PatientAdmissionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AdmitPatientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PatientAdmissionDto> Handle(AdmitPatientCommand request, CancellationToken cancellationToken)
    {

        if (!string.IsNullOrEmpty(request.RoomId))
        {
            var roomExists = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId);
            if (roomExists == null)
            {
                throw new KeyNotFoundException($"Room with ID '{request.RoomId}' does not exist.");
            }
        }
        // 1. Validate Bed Availability Early
        Bed? bed = null;
        if (!string.IsNullOrEmpty(request.BedId))
        {
            bed = await _unitOfWork.Beds.GetByIdAsync(request.BedId)
                ?? throw new KeyNotFoundException($"Bed ID '{request.BedId}' not found.");

            if (bed.Status != BedStatus.Available)
                throw new BedNotAvailableException(bed.Id, bed.BedNumber, bed.RoomId);
        }

        // 2. Patient Retrieval or Creation
        var patient = await _unitOfWork.Patients.GetByNationalIdAsync(request.NationalId, cancellationToken);

        if (patient != null)
        {
            // 3. Validation: Check if existing patient is already admitted BEFORE updating anything
            var activeAdmission = await _unitOfWork.Admissions.GetActiveAdmissionsByPatientIdAsync(patient.Id);
            if (activeAdmission != null)
            {
                throw new PatientAlreadyAdmittedException(patient.Id, patient.Name);
            }

            // Update existing patient info if needed
            UpdatePatientDetails(patient, request);
            await _unitOfWork.Patients.UpdateAsync(patient);
        }
        else
        {
            // Create new patient
            patient = CreateNewPatient(request);
            await _unitOfWork.Patients.AddAsync(patient, cancellationToken);
        }

        // 4. Create Admission
        var admission = new AdmissionEntity
        {
            PatientId = patient.Id,
            DoctorId = request.DoctorId,
            AdmissionDate = DateTime.UtcNow,
            InitialDiagnosis = request.InitialDiagnosis,
            Status = AdmissionStatus.Active,
            BedId = request.BedId,
            RoomId = request.RoomId
        };

        await _unitOfWork.Admissions.AddAsync(admission, cancellationToken);

        // 5. Update Bed Status
        if (bed != null)
        {
            bed.Status = BedStatus.Occupied;
            bed.CurrentAdmissionId = admission.Id;
            await _unitOfWork.Beds.UpdateAsync(bed);
        }

        // 6. Persistence
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PatientAdmissionDto>(admission);
    }

    private static Patient CreateNewPatient(AdmitPatientCommand request)
    {
        return new Patient
        {
            Name = request.Name,
            FileNumber = request.NationalId, // Usually file number has its own logic, but keeping your logic
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            Email = request.Email ?? string.Empty,
            PhoneNumber = request.Phone ?? string.Empty,
            BloodType = request.BloodType,
            DiagnosisSummary = request.DiagnosisSummary,
            NationalId = request.NationalId,
            Address = new Address(
                request.Street,
                request.City,
                request.State,
                request.ZipCode ?? string.Empty,
                request.Country ?? string.Empty)
        };
    }

    private static void UpdatePatientDetails(Patient patient, AdmitPatientCommand request)
    {
        if (!string.IsNullOrWhiteSpace(request.DiagnosisSummary))
            patient.DiagnosisSummary = request.DiagnosisSummary;

        if (!string.IsNullOrWhiteSpace(request.Phone))
            patient.PhoneNumber = request.Phone;

        // Potential address update logic could go here
    }
}